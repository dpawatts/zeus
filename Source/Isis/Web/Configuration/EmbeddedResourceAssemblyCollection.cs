using System.Configuration;

namespace Isis.Web.Configuration
{
	[ConfigurationCollection(typeof(EmbeddedResourceAssembly), CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public class EmbeddedResourceAssemblyCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new EmbeddedResourceAssembly();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((EmbeddedResourceAssembly) element).Assembly;
		}
	}
}