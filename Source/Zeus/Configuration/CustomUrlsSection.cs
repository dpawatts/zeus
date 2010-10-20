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
	}
}