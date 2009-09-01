using Zeus.AddIns.Blogs.Services;
using Zeus.Engine;
using Zeus.Plugin;

namespace Zeus.AddIns.Blogs
{
	[AutoInitialize]
	public class BlogsInitializer : IPluginInitializer
	{
		public void Initialize(ContentEngine engine)
		{
			engine.AddComponent("zeus.addins.blogs.blogService", typeof(BlogService));
		}
	}
}