using System;
using System.Configuration;

namespace Zeus.Configuration
{
	public class AdminSection : ConfigurationSection
	{
		[ConfigurationProperty("name", DefaultValue = "[None]")]
		public string Name
		{
			get { return (string) base["name"]; }
			set { base["name"] = value; }
		}

		[ConfigurationProperty("hideBranding", DefaultValue = false)]
		public bool HideBranding
		{
			get { return (bool) base["hideBranding"]; }
			set { base["hideBranding"] = value; }
		}
	}
}
