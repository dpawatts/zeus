using System.Configuration;

namespace Zeus.Configuration
{
	[ConfigurationCollection(typeof(MenuPluginElement), AddItemName = "add")]
	public class MenuPluginCollection : ConfigurationElementCollection
	{
		protected override string ElementName
		{
			get { return "add"; }
		}

		public void Add(MenuPluginElement item)
		{
			BaseAdd(item);
		}

		public void Clear()
		{
			BaseClear();
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new MenuPluginElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((MenuPluginElement) element).Name;
		}
	}
}