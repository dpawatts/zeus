using System.Configuration;

namespace Zeus.Templates.Configuration
{
	public class AntiSpamElement : ConfigurationElement
	{
		[ConfigurationProperty("reCaptcha")]
		public ReCaptchaElement ReCaptcha
		{
			get { return (ReCaptchaElement)base["reCaptcha"]; }
			set { base["reCaptcha"] = value; }
		}

		[ConfigurationProperty("akismet")]
		public AkismetElement Akismet
		{
			get { return (AkismetElement)base["akismet"]; }
			set { base["akismet"] = value; }
		}
	}
}