using System.Configuration;

namespace Isis.Web.Configuration
{
	public class EmbeddedResourceAssembly : ConfigurationElement
	{
		[ConfigurationProperty("assembly", IsRequired = true, IsKey = true, DefaultValue = "")]
		public string Assembly
		{
			get { return (string) base["assembly"]; }
			set { base["assembly"] = value; }
		}

		[ConfigurationProperty("path", IsRequired = true, DefaultValue = "")]
		public string Path
		{
			get { return (string) base["path"]; }
			set { base["path"] = value; }
		}
	}
}