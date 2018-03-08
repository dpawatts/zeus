using System.ComponentModel;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	public enum PaymentMethod
	{
        [Description("Sage Pay")]
		SagePay,

        [Description("PayPal Express")]
		PayPalExpress
	}
}