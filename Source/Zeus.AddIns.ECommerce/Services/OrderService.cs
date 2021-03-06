﻿using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.AddIns.ECommerce.ContentTypes;
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
		private readonly IPaymentGateway _paymentGateway;
		private readonly IOrderMailService _orderMailService;

		public OrderService(IPersister persister, IWebContext webContext,
			IPaymentGateway paymentGateway, IOrderMailService orderMailService)
		{
			_persister = persister;
			_webContext = webContext;
			_paymentGateway = paymentGateway;
			_orderMailService = orderMailService;
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

		public IEnumerable<PaymentCardType> GetSupportedCardTypes()
		{
			return Enum.GetValues(typeof(PaymentCardType)).Cast<PaymentCardType>()
				.Where(paymentCardType => _paymentGateway.SupportsCardType(paymentCardType));
		}

		public Order PlaceOrder(IECommerceConfiguration configuration, string cardNumber, string cardVerificationCode,
			DeliveryMethod deliveryMethod, decimal deliveryPrice, Address billingAddress,
			Address shippingAddress, PaymentCard paymentCard, string emailAddress,
			string telephoneNumber, string mobileTelephoneNumber,
            IEnumerable<OrderItem> items,
            decimal discountAmount, string discountDescription,
            decimal totalVatPrice, decimal totalPrice)
		{
			// Convert shopping basket into order, with unpaid status.
			Order order = new Order
			{
				User = (_webContext.User != null && (_webContext.User is WebPrincipal)) ? ((WebPrincipal) _webContext.User).MembershipUser : null,
				DeliveryMethod = deliveryMethod,
				BillingAddress = billingAddress,
				ShippingAddress = shippingAddress,
				PaymentCard = paymentCard,
				EmailAddress = emailAddress,
				TelephoneNumber = telephoneNumber,
				MobileTelephoneNumber = mobileTelephoneNumber,
				Status = OrderStatus.Unpaid,
                TotalDeliveryPrice = deliveryPrice,
                DiscountAmount = discountAmount,
                DiscountDescription = discountDescription,
                TotalVatPrice = totalVatPrice,
                TotalPrice = totalPrice
			};
			foreach (OrderItem orderItem in items)
				orderItem.AddTo(order);
			order.AddTo(configuration.Orders);
			_persister.Save(order);

            order.Title = "Order #" + order.ID;
            _persister.Save(order);

            // Process payment.
            PaymentRequest paymentRequest = new PaymentRequest(
                PaymentTransactionType.Payment, order.ID.ToString(), order.TotalPrice, order.Title,
                order.BillingAddress, order.ShippingAddress,
                order.PaymentCard, cardNumber, cardVerificationCode, 
                order.TelephoneNumber, order.EmailAddress, _webContext.Request.UserHostAddress);
			
            PaymentResponse paymentResponse = _paymentGateway.TakePayment(paymentRequest);
            
            if (paymentResponse.Success)
			{
				// Update order status to Paid.
				order.Status = OrderStatus.Paid;
				_persister.Save(order);

				// Send email to customer and vendor.

                //commented out by Dave - why would this be a centralised library - like we don't need to change this for project to project?

				//_orderMailService.SendOrderConfirmationToCustomer(configuration, order);
				//_orderMailService.SendOrderConfirmationToVendor(configuration, order);
			}
			else
			{
				throw new ZeusECommerceException(paymentResponse.Message);
			}

			return order;
		}

        public Order PlaceOrderWithoutPayment(IECommerceConfiguration configuration, DeliveryMethod deliveryMethod, 
            decimal deliveryPrice, Address billingAddress,
            Address shippingAddress, PaymentCard paymentCard, string emailAddress,
            string telephoneNumber, string mobileTelephoneNumber,
            IEnumerable<OrderItem> items,
            decimal discountAmount, string discountDescription,
            decimal totalVatPrice, decimal totalPrice)
        {
			try
			{

				// Convert shopping basket into order, with unpaid status.
				Order order = new Order
				{
					User = (_webContext.User != null && (_webContext.User is WebPrincipal)) ? ((WebPrincipal)_webContext.User).MembershipUser : null,
					DeliveryMethod = deliveryMethod,
					BillingAddress = billingAddress,
					ShippingAddress = shippingAddress,
					PaymentCard = paymentCard,
					EmailAddress = emailAddress,
					TelephoneNumber = telephoneNumber,
					MobileTelephoneNumber = mobileTelephoneNumber,
					Status = OrderStatus.Unpaid,
                    TotalDeliveryPrice = deliveryPrice,
                    DiscountAmount = discountAmount,
                    DiscountDescription = discountDescription,
                    TotalVatPrice = totalVatPrice,
                    TotalPrice = totalPrice
				};
				foreach (OrderItem orderItem in items)
					orderItem.AddTo(order);

				order.AddTo(configuration.Orders);

                order.Status = OrderStatus.Unpaid;
				_persister.Save(order);

                if (_webContext.User != null && !string.IsNullOrEmpty(_webContext.User.Identity.Name))
                {
                    order.Title = _webContext.User.Identity.Name + " for " + items.First().Title;
                }
                else
                {
                    order.Title = "Order #" + order.ID;
                }

                _persister.Save(order);

				return order;
			}
			catch (System.Exception ex)
			{ 
				throw(new System.Exception("Error creating order: \n" + ex.Message + "\n" + ex.StackTrace));
			}			
        }

		public Order PlaceOrder(Shop shop, string cardNumber, string cardVerificationCode,
			ShoppingBasket shoppingBasket)
		{
			List<OrderItem> items = new List<OrderItem>();
			foreach (IShoppingBasketItem shoppingBasketItem in items)
			{
				ProductOrderItem orderItem = new ProductOrderItem
				{
					WeakProductLink = shoppingBasketItem.Product.ID,
					ProductTitle = shoppingBasketItem.Product.Title,
					Quantity = shoppingBasketItem.Quantity,
					Price = shoppingBasketItem.VariationPermutation != null && shoppingBasketItem.VariationPermutation.PriceOverride.HasValue ? shoppingBasketItem.VariationPermutation.PriceOverride.Value : shoppingBasketItem.Product.CurrentPrice,
                    VATable = !(shoppingBasketItem.Product.VatZeroRated)
				};
				if (shoppingBasketItem.Variations != null)
					foreach (Variation variation in shoppingBasketItem.Variations)
						orderItem.Variations.Add(variation.VariationSet.Title + ": " + variation.Title);
				items.Add(orderItem);
			}

			Order order = PlaceOrder(shop, cardNumber, cardVerificationCode, shoppingBasket.DeliveryMethod,
				shoppingBasket.DeliveryMethod.Price, (Address) shoppingBasket.BillingAddress.Clone(true),
				(Address) (shoppingBasket.ShippingAddress ?? shoppingBasket.BillingAddress).Clone(true),
				(PaymentCard) shoppingBasket.PaymentCard.Clone(true), shoppingBasket.EmailAddress,
				shoppingBasket.TelephoneNumber, shoppingBasket.MobileTelephoneNumber,
				items,
                shoppingBasket.Discount.DiscountAmount(shoppingBasket.SubTotalPrice),
                shoppingBasket.Discount.DescriptionForAdministrators, 
                shoppingBasket.TotalDeliveryPrice, shoppingBasket.TotalPrice);

			// Clear shopping basket.
			_persister.Delete(shoppingBasket);

			return order;
		}
	}
}