using System.Collections.Generic;
using Zeus.AddIns.ECommerce.ContentTypes;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.PaymentGateways;

namespace Zeus.AddIns.ECommerce.Services
{
	public interface IOrderService
	{
		string GetMaskedCardNumber(string cardNumber);
		IEnumerable<PaymentCardType> GetSupportedCardTypes();

		Order PlaceOrder(IECommerceConfiguration configuration, string cardNumber, string cardVerificationCode,
			DeliveryMethod deliveryMethod, decimal deliveryPrice, Address billingAddress,
			Address shippingAddress, PaymentCard paymentCard, string emailAddress,
			string telephoneNumber, string mobileTelephoneNumber,
			IEnumerable<OrderItem> items);

        Order PlaceOrderWithoutPayment(IECommerceConfiguration configuration, DeliveryMethod deliveryMethod,
            decimal deliveryPrice, Address billingAddress,
            Address shippingAddress, PaymentCard paymentCard, string emailAddress,
            string telephoneNumber, string mobileTelephoneNumber,
            IEnumerable<OrderItem> items);

		Order PlaceOrder(Shop shop, string cardNumber, string cardVerificationCode,
			ShoppingBasket shoppingBasket);
	}
}