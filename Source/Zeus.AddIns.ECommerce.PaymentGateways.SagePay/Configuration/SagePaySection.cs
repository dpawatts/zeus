using System;
using System.Configuration;
using Zeus.Configuration;

namespace Zeus.AddIns.ECommerce.PaymentGateways.SagePay.Configuration
{
	public class SagePaySection : ConfigurationSection
	{
		[ConfigurationProperty("vpsProtocol", DefaultValue = "2.23")]
		public string VpsProtocol
		{
			get { return (string)base["vpsProtocol"]; }
			set { base["vpsProtocol"] = value; }
		}

		[ConfigurationProperty("vendorName", IsRequired = true)]
		public string VendorName
		{
			get { return (string)base["vendorName"]; }
			set { base["vendorName"] = value; }
		}

		[ConfigurationProperty("currency", DefaultValue = "GBP")]
		public string Currency
		{
			get { return (string)base["currency"]; }
			set { base["currency"] = value; }
		}

		[ConfigurationProperty("purchaseUrl", IsRequired = true)]
		public string PurchaseUrl
		{
			get { return (string)base["purchaseUrl"]; }
			set { base["purchaseUrl"] = value; }
		}
	}
}