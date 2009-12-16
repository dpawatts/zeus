using System.Configuration;

namespace Zeus.Templates.Configuration
{
	public class ReCaptchaElement : ConfigurationElement
	{
		[ConfigurationProperty("publicKey", IsRequired = true)]
		public string PublicKey
		{
			get { return (string)base["publicKey"]; }
			set { base["publicKey"] = value; }
		}

		[ConfigurationProperty("privateKey", IsRequired = true)]
		public string PrivateKey
		{
			get { return (string)base["privateKey"]; }
			set { base["privateKey"] = value; }
		}
	}
}