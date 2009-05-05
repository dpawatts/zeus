using System.Configuration;

namespace Zeus.Configuration
{
	public class RecycleBinElement : ConfigurationElement
	{
		[ConfigurationProperty("enabled", DefaultValue = true)]
		public bool Enabled
		{
			get { return (bool) base["enabled"]; }
			set { base["enabled"] = value; }
		}
	}
}