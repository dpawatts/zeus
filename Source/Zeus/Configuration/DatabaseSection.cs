using System;
using System.Configuration;

namespace Zeus.Configuration
{
	public class DatabaseSection : ConfigurationSection
	{
		[ConfigurationProperty("connectionStringName", DefaultValue = "Zeus")]
		public string ConnectionStringName
		{
			get { return (string) base["connectionStringName"]; }
			set { base["connectionStringName"] = value; }
		}

		[ConfigurationProperty("cacheEnabled", DefaultValue = false)]
		public bool CacheEnabled
		{
			get { return (bool) base["cacheEnabled"]; }
			set { base["cacheEnabled"] = value; }
		}

		[ConfigurationProperty("cacheProviderClass", DefaultValue = "NHibernate.Cache.HashtableCacheProvider, NHibernate")]
		public string CacheProviderClass
		{
			get { return (string) base["cacheProviderClass"]; }
			set { base["cacheProviderClass"] = value; }
		}
	}
}
