using System.Configuration;

namespace Zeus.Configuration
{
	public class TreeElement : ConfigurationElement
	{
		[ConfigurationProperty("tooltipsEnabled", DefaultValue = true)]
		public bool TooltipsEnabled
		{
			get { return (bool) base["tooltipsEnabled"]; }
			set { base["tooltipsEnabled"] = value; }
		}
	}
}