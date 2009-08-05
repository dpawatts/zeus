using System;
using System.Collections;
using System.Security.Principal;
using System.Web;

namespace Isis.Web
{
	public class WebContext : IWebContext
	{
		/// <summary>Provides access to HttpContext.Current.</summary>
		protected virtual HttpContext CurrentHttpContext
		{
			get
			{
				if (HttpContext.Current == null)
					throw new Exception("Tried to retrieve HttpContext.Current but it's null. This may happen when working outside a request or when doing stuff after the context has been recycled.");
				return HttpContext.Current;
			}
		}

		public IHttpHandler Handler
		{
			get { return CurrentHttpContext.Handler; }
		}

		public bool IsWeb
		{
			get { return true; }
		}

		public Url LocalUrl
		{
			get { return Url.Parse(CurrentHttpContext.Request.RawUrl); }
		}

		/// <summary>Gets a dictionary of request scoped items.</summary>
		public IDictionary RequestItems
		{
			get { return CurrentHttpContext.Items; }
		}

		/// <summary>The current request object.</summary>
		public HttpRequestBase Request
		{
			get { return new HttpRequestWrapper(CurrentHttpContext.Request); }
		}

		/// <summary>The current request object.</summary>
		public HttpResponseBase Response
		{
			get { return new HttpResponseWrapper(CurrentHttpContext.Response); }
		}

		public Url Url
		{
			get { return new Url(Request.Url.Scheme, Request.Url.Authority, Request.RawUrl); }
		}

		/// <summary>Gets the current user in the web execution context.</summary>
		public IPrincipal User
		{
			get { return CurrentHttpContext.User; }
		}

		public string MapPath(string path)
		{
			return CurrentHttpContext.Server.MapPath(path);
		}

		public void RewritePath(string path)
		{
			CurrentHttpContext.RewritePath(path);
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