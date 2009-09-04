using System.Configuration;

namespace Zeus.Templates.Configuration
{
	[ConfigurationCollection(typeof(TemplateZone), CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
	public class TemplateZoneCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new TemplateZone();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			TemplateZone rule = (TemplateZone)element;
			return rule.Name;
		}
	}
}