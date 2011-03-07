using Ninject.Modules;

namespace $safesolutionname$.Web
{
	public class WebsiteModule : NinjectModule
	{
		public override void Load()
		{
			// Hook up services here. For example:
			// Bind<IMyService>().To<MyService>().InSingletonScope();
		}
	}
}