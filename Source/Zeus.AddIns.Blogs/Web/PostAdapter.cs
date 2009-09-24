using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Web
{
	[Controls(typeof(Post))]
	public class PostAdapter : RequestAdapter
	{
		public override void InjectCurrentPage(System.Web.IHttpHandler handler)
		{
			Engine.WebContext.Response.AddHeader("X-Pingback", Engine.WebContext.GetFullyQualifiedUrl("/services/pingbacks"));
			base.InjectCurrentPage(handler);
		}
	}
}