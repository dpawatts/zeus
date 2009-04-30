using System;
using System.Configuration;

namespace Zeus.Configuration
{
	[ConfigurationCollection(typeof(DynamicContentControl), CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public class DynamicContentControlCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new DynamicContentControl();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((DynamicContentControl) element).Name;
		}
	}
}