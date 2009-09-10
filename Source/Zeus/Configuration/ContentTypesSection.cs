using System.Configuration;

namespace Zeus.Configuration
{
	public class ContentTypesSection : ConfigurationSection
	{
		[ConfigurationProperty("rules")]
		public ContentTypeRuleCollection Rules
		{
			get { return (ContentTypeRuleCollection)base["rules"]; }
			set { base["rules"] = value; }
		}

		[ConfigurationProperty("settings")]
		public ContentTypeSettingsCollection Settings
		{
			get { return (ContentTypeSettingsCollection)base["settings"]; }
			set { base["settings"] = value; }
		}
	}
}