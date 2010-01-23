using System.Collections.Generic;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.PaymentGateways;

namespace Zeus.AddIns.ECommerce.Services
{
	public interface IOrderService
	{
		IEnumerable<PaymentCardType> GetSupportedCardTypes();
		Order PlaceOrder(Shop shop, string cardNumber, string cardVerificationCode);
	}
}