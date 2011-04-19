using System.Configuration;

namespace Zeus.Configuration
{
    [ConfigurationCollection(typeof(CustomUrlsRapidCheckElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
    public class CustomUrlsRapidCheckCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
            return new CustomUrlsRapidCheckElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
        {
            CustomUrlsRapidCheckElement rule = (CustomUrlsRapidCheckElement)element;
            return rule.Action;
        }
    }
}