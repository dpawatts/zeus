using System.Configuration;

namespace Zeus.Templates.Configuration
{
	public class TemplatesSection : ConfigurationSection
	{
		[ConfigurationProperty("mailConfiguration", DefaultValue = MailConfigSource.SystemNet)]
		public MailConfigSource MailConfiguration
		{
			get { return (MailConfigSource)base["mailConfiguration"]; }
			set { base["mailConfiguration"] = value; }
		}

		[ConfigurationProperty("antiSpam")]
		public AntiSpamElement AntiSpam
		{
			get { return (AntiSpamElement) base["antiSpam"]; }
			set { base["antiSpam"] = value; }
		}

		[ConfigurationProperty("seo")]
		public SeoElement Seo
		{
			get { return (SeoElement)base["seo"]; }
			set { base["seo"] = value; }
		}

		[ConfigurationProperty("tagging")]
		public TaggingElement Tagging
		{
			get { return (TaggingElement)base["tagging"]; }
			set { base["tagging"] = value; }
		}

		[ConfigurationProperty("userRegistration")]
		public UserRegistrationElement UserRegistration
		{
			get { return (UserRegistrationElement) base["userRegistration"]; }
			set { base["userRegistration"] = value; }
		}
	}
}