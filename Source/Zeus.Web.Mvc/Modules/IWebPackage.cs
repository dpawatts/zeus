using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace Zeus.Web.Mvc.Modules
{
	public interface IWebPackage
	{
		void Register(IKernel container, ICollection<RouteBase> routes, ICollection<IViewEngine> viewEngines);
	}
}