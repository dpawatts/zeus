using Zeus.AddIns.ECommerce.ContentTypes.Data;

namespace Zeus.AddIns.ECommerce.Services
{
	public interface IPaymentGateway
	{
		void TakePayment(ShoppingBasket shoppingBasket);
	}
}