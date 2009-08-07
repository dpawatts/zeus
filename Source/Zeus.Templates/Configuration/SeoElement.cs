using System.Configuration;

namespace Zeus.Templates.Configuration
{
	public class SeoElement : ConfigurationElement
	{
		[ConfigurationProperty("enabled", DefaultValue = false)]
		public bool Enabled
		{
			get { return (bool)base["enabled"]; }
			set { base["enabled"] = value; }
		}

		[ConfigurationProperty("htmlTitleFormat", DefaultValue = "{Title}")]
		public string HtmlTitleFormat
		{
			get { return (string)base["htmlTitleFormat"]; }
			set { base["htmlTitleFormat"] = value; }
		}

		[ConfigurationProperty("metaKeywordsFormat", DefaultValue = "{Title}")]
		public string MetaKeywordsFormat
		{
			get { return (string)base["metaKeywordsFormat"]; }
			set { base["metaKeywordsFormat"] = value; }
		}

		[ConfigurationProperty("metaDescriptionFormat", DefaultValue = "{Title} page")]
		public string MetaDescriptionFormat
		{
			get { return (string)base["metaDescriptionFormat"]; }
			set { base["metaDescriptionFormat"] = value; }
		}
	}
}