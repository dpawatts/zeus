using System.Configuration;

namespace Zeus.Configuration
{
	public class VersioningElement : ConfigurationElement
	{
		[ConfigurationProperty("enabled", DefaultValue = false)]
		public bool Enabled
		{
			get { return (bool) base["enabled"]; }
			set { base["enabled"] = value; }
		}
	}
}