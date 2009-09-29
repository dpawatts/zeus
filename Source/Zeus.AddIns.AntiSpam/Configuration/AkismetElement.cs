using System.Configuration;

namespace Zeus.AddIns.AntiSpam.Configuration
{
	public class AkismetElement : ConfigurationElement
	{
		[ConfigurationProperty("apiKey", IsRequired = true)]
		public string ApiKey
		{
			get { return (string)base["apiKey"]; }
			set { base["apiKey"] = value; }
		}

		[ConfigurationProperty("apiBaseUrl", IsRequired = true, DefaultValue = "rest.akismet.com")]
		public string ApiBaseUrl
		{
			get { return (string)base["apiBaseUrl"]; }
			set { base["apiBaseUrl"] = value; }
		}

		[ConfigurationProperty("timeout", DefaultValue = 5000)]
		public int Timeout
		{
			get { return (int)base["timeout"]; }
			set { base["timeout"] = value; }
		}
	}
}