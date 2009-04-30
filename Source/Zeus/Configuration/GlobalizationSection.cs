using System.Configuration;

namespace Zeus.Configuration
{
	public class GlobalizationSection : ConfigurationSection
	{
		/// <summary>
		/// Determines whether multi-language is enabled for this website. If set to false,
		/// no language options will appear in the admin site.
		/// </summary>
		[ConfigurationProperty("enabled", DefaultValue = false)]
		public bool Enabled
		{
			get { return (bool) base["enabled"]; }
			set { base["enabled"] = value; }
		}

		/// <summary>
		/// Determines if the browser language should define which language is used.
		/// </summary>
		[ConfigurationProperty("useBrowserLanguagePreferences", DefaultValue = false)]
		public bool UseBrowserLanguagePreferences
		{
			get { return (bool) base["useBrowserLanguagePreferences"]; }
			set { base["useBrowserLanguagePreferences"] = value; }
		}
	}
}