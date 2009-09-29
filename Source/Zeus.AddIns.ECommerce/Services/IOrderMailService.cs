using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;

namespace Zeus.AddIns.ECommerce.Services
{
	public interface IOrderMailService
	{
		void SendOrderConfirmationToCustomer(Shop shop, Order order);
		void SendOrderConfirmationToVendor(Shop shop, Order order);
	}
}