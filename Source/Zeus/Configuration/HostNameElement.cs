using System.Configuration;

namespace Zeus.Configuration
{
	public class HostNameElement : ConfigurationElement
	{
		[ConfigurationProperty("language", IsRequired = false)]
		public string Language
		{
			get { return (string) base["language"]; }
			set { base["language"] = value; }
		}

		[ConfigurationProperty("name", IsRequired = true)]
		public string Name
		{
			get { return (string) base["name"]; }
			set { base["name"] = value; }
		}
	}
}