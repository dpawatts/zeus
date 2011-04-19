using System;
using System.Configuration;

namespace Zeus.Configuration
{
    public class CustomUrlsRapidCheckElement : ConfigurationElement
	{
		[ConfigurationProperty("action", IsRequired = true)]
        public string Action
        {
            get
            {
                return (string)this["action"];
            }
            set
            {
                this["action"] = value;
            }

        }
	}
}