using System;
using System.Configuration;

namespace Zeus.Configuration
{
	public class FileSystemSection : ConfigurationSection
	{
		[ConfigurationProperty("settings")]
		public KeyValueConfigurationCollection Settings
		{
			get { return (KeyValueConfigurationCollection) base["settings"]; }
		}
	}
}
