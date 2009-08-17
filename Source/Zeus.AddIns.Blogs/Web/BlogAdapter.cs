using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Web
{
	[Controls(typeof(Blog))]
	public class BlogAdapter : RequestAdapter
	{
		public override void InjectCurrentPage(System.Web.IHttpHandler handler)
		{
			Engine.WebContext.Response.AddHeader("X-Pingback", "http://localhost:10220/Pingback.aspx");
			base.InjectCurrentPage(handler);
		}
	}
}