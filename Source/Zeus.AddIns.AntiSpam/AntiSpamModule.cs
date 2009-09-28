using System;
using System.Configuration;
using Ninject.Modules;
using Zeus.AddIns.AntiSpam.Configuration;
using Zeus.AddIns.AntiSpam.Services;

namespace Zeus.AddIns.AntiSpam
{
	public class AntiSpamModule : NinjectModule
	{
		public override void Load()
		{
			AntiSpamSection antiSpamSection = ConfigurationManager.GetSection("zeus.addIns.antiSpam") as AntiSpamSection;
			if (antiSpamSection != null && antiSpamSection.ReCaptcha != null)
			{
				Bind<ReCaptchaElement>().ToConstant(antiSpamSection.ReCaptcha);
				Bind<ICaptchaService>().To<ReCaptchaService>().InSingletonScope();
			}
		}
	}
}