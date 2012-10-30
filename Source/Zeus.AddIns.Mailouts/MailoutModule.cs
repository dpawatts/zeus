using Ninject.Modules;
using Zeus.AddIns.Mailouts.Services;

namespace Zeus.AddIns.Mailouts
{
	public class MailoutModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IMailoutService>().To<MailoutService>().InSingletonScope();
		}
	}
}