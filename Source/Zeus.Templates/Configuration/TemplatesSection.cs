using System.Configuration;

namespace Zeus.Templates.Configuration
{
	public class TemplatesSection : ConfigurationSection
	{
		[ConfigurationProperty("rules")]
		public TemplateRuleCollection Rules
		{
			get { return (TemplateRuleCollection) base["rules"]; }
			set { base["rules"] = value; }
		}

		[ConfigurationProperty("mailConfiguration", DefaultValue = MailConfigSource.SystemNet)]
		public MailConfigSource MailConfiguration
		{
			get { return (MailConfigSource)base["mailConfiguration"]; }
			set { base["mailConfiguration"] = value; }
		}

		[ConfigurationProperty("seo")]
		public SeoElement Seo
		{
			get { return (SeoElement)base["seo"]; }
			set { base["seo"] = value; }
		}

		[ConfigurationProperty("availableZones")]
		public TemplateZoneCollection AvailableZones
		{
			get { return (TemplateZoneCollection)base["availableZones"]; }
			set { base["availableZones"] = value; }
		}
	}
}
