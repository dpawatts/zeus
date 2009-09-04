using Ninject.Modules;
using Zeus.AddIns.Blogs.Services;

namespace Zeus.AddIns.Blogs
{
	public class BlogsModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ICommentService>().To<CommentService>().InSingletonScope();
		}
	}
}