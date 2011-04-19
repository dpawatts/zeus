using System.Configuration;

namespace Zeus.Configuration
{
    public class CustomUrlsSection : ConfigurationSection
	{
        [ConfigurationProperty("parentIDs")]
        public CustomUrlsIDsCollection ParentIDs
		{
			get { return (CustomUrlsIDsCollection) base["parentIDs"]; }
			set { base["parentIDs"] = value; }
        }

        [ConfigurationProperty("rapidCheck")]
        public CustomUrlsRapidCheckCollection RapidCheck
        {
            get { return (CustomUrlsRapidCheckCollection)base["rapidCheck"]; }
            set { base["rapidCheck"] = value; }
        }

	}
}