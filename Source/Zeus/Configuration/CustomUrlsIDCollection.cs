using System.Configuration;

namespace Zeus.Configuration
{
    [ConfigurationCollection(typeof(CustomUrlsIDElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
    public class CustomUrlsIDsCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
            return new CustomUrlsIDElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
        {
            CustomUrlsIDElement rule = (CustomUrlsIDElement)element;
            return rule.ID;
        }
    }
}