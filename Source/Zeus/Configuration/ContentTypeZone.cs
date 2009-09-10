using System.Configuration;

namespace Zeus.Configuration
{
	public class ContentTypeZone : ConfigurationElement
	{
		[ConfigurationProperty("name", DefaultValue = "")]
		public string Name
		{
			get { return (string)base["name"]; }
			set { base["name"] = value; }
		}

		[ConfigurationProperty("title", DefaultValue = "")]
		public string Title
		{
			get { return (string)base["title"]; }
			set { base["title"] = value; }
		}
	}
}