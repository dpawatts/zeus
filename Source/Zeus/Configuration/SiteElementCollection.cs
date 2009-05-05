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

		public void Add(SiteElement site)
		{
			BaseAdd(site);
		}

		public void Clear()
		{
			BaseClear();
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