using System;
using System.Configuration;

namespace Zeus.Configuration
{
	public class CustomUrlsIDElement : ConfigurationElement
	{
        public int ID
        {
            get
            {
                return (int)this["value"];
            }
            set
            {
                this["value"] = value;
            }

        }
	}
}