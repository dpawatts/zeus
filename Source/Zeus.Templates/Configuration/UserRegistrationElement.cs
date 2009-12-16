using System.Configuration;

namespace Zeus.Templates.Configuration
{
	public class UserRegistrationElement : ConfigurationElement
	{
		[ConfigurationProperty("emailVerificationRequired", DefaultValue = false)]
		public bool EmailVerificationRequired
		{
			get { return (bool) base["emailVerificationRequired"]; }
			set { base["emailVerificationRequired"] = value; }
		}

		[ConfigurationProperty("defaultRole", DefaultValue = "WebsiteUsers")]
		public string DefaultRole
		{
			get { return (string) base["defaultRole"]; }
			set { base["defaultRole"] = value; }
		}
	}
}