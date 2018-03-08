using System.Configuration;

namespace Zeus.Templates.Configuration
{
	public class TaggingElement : ConfigurationElement
	{
		[ConfigurationProperty("enabled", DefaultValue = false)]
		public bool Enabled
		{
			get { return (bool)base["enabled"]; }
			set { base["enabled"] = value; }
		}
	}
}