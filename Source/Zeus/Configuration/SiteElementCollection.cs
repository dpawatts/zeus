using System.Configuration;

namespace Zeus.Configuration
{
	[ConfigurationCollection(typeof(SiteElement), AddItemName = "site")]
	public class SiteElementCollection : ConfigurationElementCollection
	{
		protected override string ElementName
		{
			get { return "site"; }
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new SiteElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SiteElement) element).ID;
		}
	}
}