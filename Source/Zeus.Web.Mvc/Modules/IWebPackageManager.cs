using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Zeus.Web.Mvc.Modules
{
	public interface IWebPackageManager
	{
		void LocatePackages();
		void RegisterPackages(ICollection<RouteBase> routes, ICollection<IViewEngine> engines);
	}
}