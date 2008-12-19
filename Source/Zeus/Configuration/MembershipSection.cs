using System;
using System.Configuration;

namespace Zeus.Configuration
{
	public class MembershipSection : ConfigurationSection
	{
		[ConfigurationProperty("customUserClass", DefaultValue = null)]
		public string CustomUserClass
		{
			get { return (string) base["customUserClass"]; }
			set { base["customUserClass"] = value; }
		}
	}
}
