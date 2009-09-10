using System.Configuration;

namespace Zeus.Configuration
{
	[ConfigurationCollection(typeof(ContentTypeSettingsElement), AddItemName = "contentType", CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
	public class ContentTypeSettingsCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ContentTypeSettingsElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			ContentTypeSettingsElement rule = (ContentTypeSettingsElement)element;
			return rule.Type;
		}
	}
}