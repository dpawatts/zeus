using System.Configuration;

namespace Zeus.Templates.Configuration
{
	public class TemplatesSection : ConfigurationSection
	{
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
		private static readonly ConfigurationProperty _propRules = new ConfigurationProperty(null, typeof(TemplateRuleCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		static TemplatesSection()
		{
			_properties.Add(_propRules);
		}

		protected override ConfigurationPropertyCollection Properties
		{
			get { return _properties; }
		}

		[ConfigurationProperty("", IsDefaultCollection = true)]
		public TemplateRuleCollection Rules
		{
			get { return (TemplateRuleCollection) base[_propRules]; }
		}

		[ConfigurationProperty("mailConfiguration", DefaultValue = MailConfigSource.SystemNet)]
		public MailConfigSource MailConfiguration
		{
			get { return (MailConfigSource)base["mailConfiguration"]; }
			set { base["mailConfiguration"] = value; }
		}
	}
}
