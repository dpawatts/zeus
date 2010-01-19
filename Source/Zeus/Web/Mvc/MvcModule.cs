using Ninject.Modules;
using System.Web.Mvc;
using Zeus.Web.Mvc.Html;

namespace Zeus.Web.Mvc
{
	public class MvcModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ITemplateRenderer>().To<TemplateRenderer>().InSingletonScope();
			Bind<IControllerMapper>().To<ControllerMapper>().InSingletonScope();
			Bind<IControllerFactory>().To<ControllerFactory>().InSingletonScope();
		}
	}
}