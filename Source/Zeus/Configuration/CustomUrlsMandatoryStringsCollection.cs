using System.Configuration;

namespace Zeus.Configuration
{
    [ConfigurationCollection(typeof(CustomUrlsMandatoryStringsElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
    public class CustomUrlsMandatoryStringsCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
            return new CustomUrlsMandatoryStringsElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
        {
            CustomUrlsMandatoryStringsElement rule = (CustomUrlsMandatoryStringsElement)element;
            return rule.Value;
        }
    }
}