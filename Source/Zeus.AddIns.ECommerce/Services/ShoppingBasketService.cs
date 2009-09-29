using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Isis.Collections.Generic;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.PaymentGateways;
using Zeus.Net.Mail;
using Zeus.Persistence;
using Zeus.Web;
using Zeus.Web.TextTemplating;

namespace Zeus.AddIns.ECommerce.Services
{
	public class ShoppingBasketService : IShoppingBasketService
	{
		private readonly IPersister _persister;
		private readonly IWebContext _webContext;
		private readonly IFinder<ShoppingBasket> _finder;
		private readonly IPaymentGateway _paymentGateway;
		private readonly IOrderMailService _orderMailService;

		public ShoppingBasketService(IPersister persister, IWebContext webContext, IFinder<ShoppingBasket> finder,
			IPaymentGateway paymentGateway, IOrderMailService orderMailService)
		{
			_persister = persister;
			_webContext = webContext;
			_finder = finder;
			_paymentGateway = paymentGateway;
			_orderMailService = orderMailService;
		}

		public bool IsValidVariationPermutation(Product product, IEnumerable<Variation> variations)
		{
			return product.VariationConfigurations.Any(vc => vc.Available
				&& EnumerableUtility.EqualsIgnoringOrder(vc.Permutation.Variations.Cast<Variation>(), variations));
		}

		public IEnumerable<PaymentCardType> GetSupportedCardTypes()
		{
			foreach (PaymentCardType paymentCardType in Enum.GetValues(typeof(PaymentCardType)))
				if (_paymentGateway.SupportsCardType(paymentCardType))
					yield return paymentCardType;
		}

		public void AddItem(Shop shop, Product product, IEnumerable<Variation> variations)
		{
			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop);

			// If card is already in basket, just increment quantity, otherwise create a new item.
			ShoppingBasketItem item = shoppingBasket.GetChildren<ShoppingBasketItem>().SingleOrDefault(i => i.Product == product && EnumerableUtility.Equals(i.Variations, variations));
			if (item == null)
			{
				VariationPermutation variationPermutation = new VariationPermutation();
				foreach (Variation variation in variations)
					variationPermutation.Variations.Add(variation);
				item = new ShoppingBasketItem { Product = product, VariationPermutation = variationPermutation, Quantity = 1 };
				item.AddTo(shoppingBasket);
			}
			else
			{
				item.Quantity += 1;
			}

			_persister.Save(shoppingBasket);
		}

		public void RemoveItem(Shop shop, Product product, VariationPermutation variationPermutation)
		{
			UpdateQuantity(shop, product, variationPermutation, 0);
		}

		public void UpdateQuantity(Shop shop, Product product, VariationPermutation variationPermutation, int newQuantity)
		{
			if (newQuantity < 0)
				throw new ArgumentOutOfRangeException("newQuantity", "Quantity must be greater than or equal to 0.");

			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop);
			ShoppingBasketItem item = shoppingBasket.GetChildren<ShoppingBasketItem>().SingleOrDefault(i => i.Product == product && i.VariationPermutation == variationPermutation);

			if (item == null)
				return;

			if (newQuantity == 0)
				shoppingBasket.Children.Remove(item);
			else
				item.Quantity = newQuantity;

			_persister.Save(shoppingBasket);
		}

		public IShoppingBasket GetBasket(Shop shop)
		{
			return GetCurrentShoppingBasketInternal(shop);
		}

		public void ClearBasket(Shop shop)
		{
			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop);
			if (shoppingBasket != null)
			{
				_persister.Delete(shoppingBasket);
				_webContext.Response.Cookies.Remove(GetCookieKey(shop));
			}
		}

		private static string GetCookieKey(Shop shop)
		{
			return "ZeusECommerce" + shop.ID;
		}

		private ShoppingBasket GetShoppingBasketFromCookie(Shop shop)
		{
			HttpCookie cookie = _webContext.Request.Cookies[GetCookieKey(shop)];
			if (cookie == null)
				return null;

			string shopperID = cookie.Value;
			return _finder.Items().SingleOrDefault(sb => sb.Name == shopperID);
		}

		private ShoppingBasket GetCurrentShoppingBasketInternal(Shop shop)
		{
			ShoppingBasket shoppingBasket = GetShoppingBasketFromCookie(shop);

			if (shoppingBasket != null)
			{
				// Check that products in shopping cart still exist, and if not remove those shopping cart items.
				List<ShoppingBasketItem> itemsToRemove = new List<ShoppingBasketItem>();
				foreach (ShoppingBasketItem item in shoppingBasket.GetChildren<ShoppingBasketItem>())
					if (item.Product == null)
						itemsToRemove.Add(item);
				foreach (ShoppingBasketItem item in itemsToRemove)
					shoppingBasket.Children.Remove(item);
				_persister.Save(shoppingBasket);
			}
			else
			{
				shoppingBasket = new ShoppingBasket { Name = Guid.NewGuid().ToString() };
				shoppingBasket.DeliveryMethod = shop.DeliveryMethods.GetChildren<DeliveryMethod>().FirstOrDefault();
				shoppingBasket.AddTo(shop.ShoppingBaskets);
				_persister.Save(shoppingBasket);

				HttpCookie cookie = new HttpCookie(GetCookieKey(shop), shoppingBasket.Name)
				{
					Expires = DateTime.Now.AddYears(1)
				};
				_webContext.Response.Cookies.Add(cookie);
			}

			return shoppingBasket;
		}

		/// <summary>
		/// Masks the credit card using XXXXXX and appends the last 4 digits
		/// </summary>
		public string GetMaskedCardNumber(string cardNumber)
		{
			string result = "****";
			if (cardNumber.Length > 8)
			{
				string lastFour = cardNumber.Substring(cardNumber.Length - 4, 4);
				result = "**** **** **** " + lastFour;
			}
			return result;
		}

		public void SaveBasket(Shop shop)
		{
			_persister.Save(GetCurrentShoppingBasketInternal(shop));
		}

		public Order PlaceOrder(Shop shop, string cardNumber, string cardVerificationCode)
		{
			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop);

			// Convert shopping basket into order, with unpaid status.
			Order order = new Order
			{
				DeliveryMethod = shoppingBasket.DeliveryMethod,
				DeliveryPrice = shoppingBasket.DeliveryMethod.Price,
				BillingAddress = (Address)shoppingBasket.BillingAddress.Clone(true),
				ShippingAddress = (Address)(shoppingBasket.ShippingAddress ?? shoppingBasket.BillingAddress).Clone(true),
				PaymentCard = (PaymentCard)shoppingBasket.PaymentCard.Clone(true),
				EmailAddress = shoppingBasket.EmailAddress,
				TelephoneNumber = shoppingBasket.TelephoneNumber,
				MobileTelephoneNumber = shoppingBasket.MobileTelephoneNumber,
				Status = OrderStatus.Unpaid
			};
			foreach (ShoppingBasketItem shoppingBasketItem in shoppingBasket.Items)
			{
				OrderItem orderItem = new OrderItem
				{
					Product = shoppingBasketItem.Product,
					ProductTitle = shoppingBasketItem.Product.Title,
					Quantity = shoppingBasketItem.Quantity,
					Price = shoppingBasketItem.Product.CurrentPrice
				};
				foreach (Variation variation in shoppingBasketItem.Variations)
					orderItem.Variations.Add(variation.VariationSet.Title + ": " + variation.Title);
				orderItem.AddTo(order);
			}
			order.AddTo(shop.Orders);
			_persister.Save(order);

			// Process payment.
			PaymentRequest paymentRequest = new PaymentRequest(order.BillingAddress, order.ShippingAddress, order.ID.ToString(), order.TotalPrice,
				"Order #" + order.ID, order.PaymentCard.NameOnCard, cardNumber, order.PaymentCard.ValidFrom, order.PaymentCard.ValidTo,
				order.PaymentCard.IssueNumber, cardVerificationCode, order.PaymentCard.CardType, order.TelephoneNumber,
				order.EmailAddress, _webContext.Request.UserHostAddress);
			PaymentResponse paymentResponse = _paymentGateway.TakePayment(paymentRequest);

			if (paymentResponse.Success)
			{
				// Clear shopping basket.
				_persister.Delete(shoppingBasket);

				// Update order status to Paid.
				order.Status = OrderStatus.Paid;
				_persister.Save(order);

				// Send email to customer and vendor.
				_orderMailService.SendOrderConfirmationToCustomer(shop, order);
				_orderMailService.SendOrderConfirmationToVendor(shop, order);
			}
			else
			{
				throw new ZeusECommerceException(paymentResponse.Message);
			}

			return order;
		}
	}
}