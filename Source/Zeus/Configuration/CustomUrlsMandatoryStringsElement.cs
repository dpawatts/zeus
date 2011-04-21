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
	}
}