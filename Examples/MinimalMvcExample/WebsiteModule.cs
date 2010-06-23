using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;

namespace Zeus.Examples.MinimalMvcExample
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
