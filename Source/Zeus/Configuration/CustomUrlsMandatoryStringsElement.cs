using System;
using System.Configuration;

namespace Zeus.Configuration
{
    public class CustomUrlsMandatoryStringsElement : ConfigurationElement
	{
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return (string)this["value"];
            }
            set
            {
                this["value"] = value;
            }

        }

        [ConfigurationProperty("isRegex", IsRequired = true)]
        public bool IsRegex
        {
            get
            {
                return (bool)this["isRegex"];
            }
            set
            {
                this["isRegex"] = value;
            }

        }
	}
}