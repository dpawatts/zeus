using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Zeus.BaseLibrary.DependencyInjection;

namespace Zeus.Web.Mvc.Modules
{
	public class WebPackageManager : IWebPackageManager
	{
		private readonly IKernel _kernel;

		public WebPackageManager(IKernel kernel)
		{
			_kernel = kernel;
		}

		public void LocatePackages()
		{
			// NOTE - this could be a place to rely on a package locating service 
			// rather than code in the discovery strategy

			var searchPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath);
			IEnumerable<string> filenames = Directory.GetFiles(searchPath, "*.dll");
			DependencyInjectionUtility.RegisterAllComponents<IWebPackage>(_kernel, filenames);
		}

		public void RegisterPackages(ICollection<RouteBase> routes, ICollection<IViewEngine> engines)
		{
			foreach (IWebPackage package in _kernel.GetAll<IWebPackage>())
				package.Register(_kernel, routes, engines);
		}
	}
}