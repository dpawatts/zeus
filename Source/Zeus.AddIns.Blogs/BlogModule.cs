using System;
using Ninject.Modules;
using Zeus.AddIns.Blogs.Services;

namespace Zeus.AddIns.Blogs
{
	public class ForumModule : NinjectModule
	{
		public override void Load()
		{
			Bind<BlogInitializer>().To<BlogInitializer>().InSingletonScope();
		}
	}
}