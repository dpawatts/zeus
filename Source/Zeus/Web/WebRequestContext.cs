using System;
using System.Web;
using System.Security.Principal;

namespace Zeus.Web
{
	public class WebRequestContext : IWebContext
	{
		public ContentItem CurrentPage
		{
			get { return HttpContext.Current.Items["CurrentPage"] as ContentItem; }
			set { HttpContext.Current.Items["CurrentPage"] = value; }
		}

		public Url LocalUrl
		{
			get { return Url.Parse(HttpContext.Current.Request.RawUrl); }
		}

		/// <summary>Gets the current user in the web execution context.</summary>
		public IPrincipal User
		{
			get { return HttpContext.Current.User; }
		}

		public string MapPath(string path)
		{
			return HttpContext.Current.Server.MapPath(path);
		}

		public string ToAbsolute(string virtualPath)
		{
			return Url.ToAbsolute(virtualPath);
		}

		public string ToAppRelative(string virtualPath)
		{
			return VirtualPathUtility.ToAppRelative(virtualPath);
		}
	}
}
