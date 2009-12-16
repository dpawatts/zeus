using System.Configuration;

namespace Zeus.Configuration
{
	public class CredentialsSection : ConfigurationSection
	{
		[ConfigurationProperty("passwordEncryptionEnabled", DefaultValue = true)]
		public bool PasswordEncryptionEnabled
		{
			get { return (bool) base["passwordEncryptionEnabled"]; }
			set { base["passwordEncryptionEnabled"] = value; }
		}
	}
}