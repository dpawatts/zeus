using System.Configuration;
using Ninject.Modules;
using Zeus.Net.Mail;
using Zeus.Templates.Configuration;
using Zeus.Templates.Services;
using Zeus.Templates.Services.AntiSpam;
using Zeus.Templates.Services.Syndication;

namespace Zeus.Templates
{
	public class TemplatesModule : NinjectModule
	{
		public override void Load()
		{
			Bind<BBCodeService>().ToSelf();
			Bind<ITagService>().To<TagService>();
			Bind<IUserRegistrationService>().To<UserRegistrationService>();

			TemplatesSection configSection = ConfigurationManager.GetSection("zeus/templates") as TemplatesSection;
			if (configSection != null)
				Bind(typeof(TemplatesSection)).ToConstant(configSection);

			if (configSection == null || configSection.MailConfiguration == MailConfigSource.SystemNet)
				Bind<IMailSender>().To<MailSender>();
			else
				Bind<IMailSender>().To<FakeMailSender>();

			Bind<SeoDefinitionAppender>().ToSelf();
			Bind<SyndicatableDefinitionAppender>().ToSelf();
			Bind<TaggingDefinitionAppender>().ToSelf();

			Bind<PageNotFoundHandler>().ToSelf().InSingletonScope();

			if (configSection != null)
			{
				AntiSpamElement antiSpamSection = configSection.AntiSpam;
				if (antiSpamSection != null)
				{
					if (antiSpamSection.ReCaptcha != null)
					{
						Bind<ReCaptchaElement>().ToConstant(antiSpamSection.ReCaptcha);
						Bind<ICaptchaService>().To<ReCaptchaService>().InSingletonScope();
					}

					if (antiSpamSection.Akismet != null)
					{
						Bind<AkismetElement>().ToConstant(antiSpamSection.Akismet);
						Bind<IAntiSpamService>().To<AkismetService>().InSingletonScope();
					}
				}
			}
		}
	}
}