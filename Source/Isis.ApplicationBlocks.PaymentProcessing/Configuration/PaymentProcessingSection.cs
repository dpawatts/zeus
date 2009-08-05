using System.Configuration;

namespace Isis.ApplicationBlocks.PaymentProcessing.Configuration
{
	public class PaymentProcessingSection : ConfigurationSection
	{
		[ConfigurationProperty("provider", IsRequired = true)]
		public string Provider
		{
			get { return this["provider"] as string; }
			set { this["provider"] = value; }
		}

		[ConfigurationProperty("", IsDefaultCollection = true)]
		public NameValueConfigurationCollection Settings
		{
			get { return (NameValueConfigurationCollection) base[""]; }
		}
	}
}