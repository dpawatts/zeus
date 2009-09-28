using System.Configuration;

namespace Zeus.AddIns.AntiSpam.Configuration
{
	public class AntiSpamSection : ConfigurationSection
	{
		[ConfigurationProperty("reCaptcha")]
		public ReCaptchaElement ReCaptcha
		{
			get { return (ReCaptchaElement)base["reCaptcha"]; }
			set { base["reCaptcha"] = value; }
		}
	}
}