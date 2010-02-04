using Zeus.AddIns.ECommerce.ContentTypes.Data;

namespace Zeus.AddIns.ECommerce.ContentTypes
{
	public interface IECommerceConfiguration
	{
		string ConfirmationEmailFrom { get; }
		string ConfirmationEmailText { get; }
		string VendorEmail { get; }

		OrderContainer Orders { get; }
	}
}