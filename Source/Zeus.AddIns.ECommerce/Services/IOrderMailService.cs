using Zeus.AddIns.ECommerce.ContentTypes;
using Zeus.AddIns.ECommerce.ContentTypes.Data;

namespace Zeus.AddIns.ECommerce.Services
{
	public interface IOrderMailService
	{
		void SendOrderConfirmationToCustomer(IECommerceConfiguration shop, Order order);
		void SendOrderConfirmationToVendor(IECommerceConfiguration shop, Order order);
	}
}