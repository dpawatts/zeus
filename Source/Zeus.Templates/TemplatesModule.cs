using Ninject.Modules;
using Zeus.Templates.Services;

namespace Zeus.Templates
{
	public class TemplatesModule : NinjectModule
	{
		public override void Load()
		{
			Bind<PageNotFoundHandler>().ToSelf().InSingletonScope();
		}
	}
}