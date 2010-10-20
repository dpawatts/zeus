using System;
using System.Configuration;

namespace Zeus.Configuration
{
	public class CustomUrlsIDElement : ConfigurationElement
	{
		[ConfigurationProperty("id", IsRequired = true)]
        public int ID
        {
            get
            {
							return (int) this["id"];
            }
            set
            {
							this["id"] = value;
            }

        }
	}
}