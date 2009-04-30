using System.Configuration;

namespace Zeus.Configuration
{
	public class DynamicContentControl : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
		public string Name
		{
			get { return (string) base["name"]; }
			set { base["name"] = value; }
		}

		[ConfigurationProperty("type", IsRequired = true, DefaultValue = "")]
		public string Type
		{
			get { return (string) base["type"]; }
			set { base["type"] = value; }
		}

		[ConfigurationProperty("description")]
		public string Description
		{
			get { return (string) base["description"]; }
			set { base["description"] = value; }
		}
	}
}