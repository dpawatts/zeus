using System.Configuration;

namespace Isis.Web.Configuration
{
	public class EmbeddedResourcesSection : ConfigurationSection
	{
		[ConfigurationProperty("virtualPathAssemblies")]
		public EmbeddedResourceAssemblyCollection VirtualPathAssemblies
		{
			get { return (EmbeddedResourceAssemblyCollection) base["virtualPathAssemblies"]; }
		}

		[ConfigurationProperty("webResourceAssemblies")]
		public EmbeddedResourceAssemblyCollection WebResourceAssemblies
		{
			get { return (EmbeddedResourceAssemblyCollection) base["webResourceAssemblies"]; }
		}
	}
}