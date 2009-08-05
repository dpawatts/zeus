using System;
using Ninject.Modules;
using Zeus.AddIns.Forums.Services;

namespace Zeus.AddIns.Forums
{
	public class ForumModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IForumService>().To<ForumService>().InSingletonScope();
		}
	}
}