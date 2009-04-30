using System.Configuration;

namespace Zeus.Configuration
{
	public class SiteElement : ConfigurationElement
	{
		[ConfigurationProperty("id", IsRequired = true, IsKey = true)]
		public string ID
		{
			get { return (string) base["id"]; }
			set { base["id"] = value; }
		}

		[ConfigurationProperty("startPageID", IsRequired = true)]
		public int StartPageID
		{
			get { return (int) base["startPageID"]; }
			set { base["startPageID"] = value; }
		}

		[ConfigurationProperty("description", IsRequired = true)]
		public string Description
		{
			get { return (string) base["description"]; }
			set { base["description"] = value; }
		}

		/// <summary>Use wildcard matching for this hostname, e.g. both sitdap.com and www.sitdap.com.</summary>
		[ConfigurationProperty("wildcards", DefaultValue = false)]
		public bool Wildcards
		{
			get { return (bool) base["wildcards"]; }
			set { base["wildcards"] = value; }
		}

		[ConfigurationProperty("siteHosts", IsRequired = false)]
		public HostNameCollection SiteHosts
		{
			get { return (HostNameCollection) base["siteHosts"]; }
		}
	}
}