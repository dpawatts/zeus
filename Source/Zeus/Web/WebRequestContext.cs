using System;
using System.Web;

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
