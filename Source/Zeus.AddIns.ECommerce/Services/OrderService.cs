using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.PaymentGateways;
using Zeus.Persistence;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.AddIns.ECommerce.Services
{
	public class OrderService : IOrderService
	{
		private readonly IPersister _persister;
		private readonly IWebContext _webContext;
		private readonly IShoppingBasketService _shoppingBasketService;
		private readonly IPaymentGateway _paymentGateway;
		private readonly IOrderMailService _orderMailService;

		public OrderService(IPersister persister, IWebContext webContext, IShoppingBasketService shoppingBasketService,
			IPaymentGateway paymentGateway, IOrderMailService orderMailService)
		{
			_persister = persister;
			_webContext = webContext;
			_shoppingBasketService = shoppingBasketService;
			_paymentGateway = paymentGateway;
			_orderMailService = orderMailService;
		}

		public IEnumerable<PaymentCardType> GetSupportedCardTypes()
		{
			return Enum.GetValues(typeof(PaymentCardType)).Cast<PaymentCardType>()
				.Where(paymentCardType => _paymentGateway.SupportsCardType(paymentCardType));
		}

		public Order PlaceOrder(Shop shop, string cardNumber, string cardVerificationCode)
		{
			ShoppingBasket shoppingBasket = (ShoppingBasket) _shoppingBasketService.GetBasket(shop);

			// Convert shopping basket into order, with unpaid status.
			Order order = new Order
			{
				User = (_webContext.User != null && (_webContext.User is WebPrincipal)) ? ((WebPrincipal) _webContext.User).MembershipUser : null,
				DeliveryMethod = shoppingBasket.DeliveryMethod,
				DeliveryPrice = shoppingBasket.DeliveryMethod.Price,
				BillingAddress = (Address) shoppingBasket.BillingAddress.Clone(true),
				ShippingAddress = (Address) (shoppingBasket.ShippingAddress ?? shoppingBasket.BillingAddress).Clone(true),
				PaymentCard = (PaymentCard) shoppingBasket.PaymentCard.Clone(true),
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