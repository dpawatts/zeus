using System.Configuration;

namespace Zeus.Configuration
{
    public class CustomUrlsSection : ConfigurationSection
	{
		[ConfigurationProperty("IDs")]
        public CustomUrlsIDsCollection IDs
		{
            get { return (CustomUrlsIDsCollection)base["IDs"]; }
            set { base["IDs"] = value; }
		}
	}
}