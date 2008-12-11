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

		[ConfigurationProperty("startPageID", DefaultValue = 1)]
		public int StartPageID
		{
			get { return (int) base["startPageID"]; }
			set { base["startPageID"] = value; }
		}
	}
}
