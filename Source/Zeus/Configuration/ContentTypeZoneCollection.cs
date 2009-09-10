using System.Configuration;

namespace Zeus.Configuration
{
	[ConfigurationCollection(typeof(ContentTypeZone), CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
	public class ContentTypeZoneCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ContentTypeZone();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			ContentTypeZone rule = (ContentTypeZone)element;
			return rule.Name;
		}
	}
}