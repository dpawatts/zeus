using Ninject.Modules;
using Zeus.AddIns.Blogs.Services;
using Zeus.AddIns.Blogs.Services.Tracking;

namespace Zeus.AddIns.Blogs
{
	public class BlogsModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IBlogService>().To<BlogService>().InSingletonScope();
			Bind<ICommentService>().To<CommentService>().InSingletonScope();
			Bind<IPingbackService>().To<PingbackService>().InSingletonScope();
			Bind<ITrackingService>().To<TrackingService>().InSingletonScope();
		}
	}
}