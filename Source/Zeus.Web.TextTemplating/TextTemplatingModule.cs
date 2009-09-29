using Ninject.Modules;

namespace Zeus.Web.TextTemplating
{
	public class TextTemplatingModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IMessageBuilder>().To<DefaultMessageBuilder>().InSingletonScope();
		}
	}
}