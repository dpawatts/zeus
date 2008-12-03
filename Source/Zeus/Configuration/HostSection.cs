using System;
using System.Configuration;

namespace Zeus.Configuration
{
	public class HostSection : ConfigurationSection
	{
		[ConfigurationProperty("rootItemID", DefaultValue = 1)]
		public int RootItemID
		{
			get { return (int) base["rootItemID"]; }
			set { base["rootItemID"] = value; }
		}
	}
}
