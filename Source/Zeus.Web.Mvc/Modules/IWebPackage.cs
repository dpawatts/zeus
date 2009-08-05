using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

namespace Zeus.Web.Mvc.Modules
{
	public interface IWebPackage
	{
		void Register(IKernel container, ICollection<RouteBase> routes, ICollection<IViewEngine> viewEngines);
	}
}