using System;
using System.Linq;
using System.Web;
using System.IO;
using Isis.Web;
using Zeus.Web.UI;
using NHibernate;

namespace Zeus.Web.Modules
{
	public class ContentModule : IHttpModule
	{
		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(context_BeginRequest);
			context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
			context.EndRequest += new EventHandler(context_EndRequest);
		}

		private void context_BeginRequest(object sender, EventArgs e)
		{
			if (Path.GetExtension(HttpContext.Current.Request.Path) != ".aspx" || HttpContext.Current.Request.Path.StartsWith("/admin/"))
				return;

			ContentItem currentItem = Zeus.Context.Current.UrlParser.ParsePage(HttpContext.Current.Request.Path);

			// If url matches a physical page, don't rewrite.
			if (!File.Exists(HttpContext.Current.Request.PhysicalPath))
			{
				// Rewrite URL.
				HttpContext.Current.RewritePath(currentItem.TemplateUrl + HttpContext.Current.Request.Url.Query);
			}

			Zeus.Context.Current.Resolve<IWebContext>().CurrentPage = currentItem;
		}

		private string CleanUrl(string url)
		{
			if (url.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase))
				url = url.Substring(0, url.Length - ".aspx".Length);
			return url;
		}

		private void context_AcquireRequestState(object sender, EventArgs e)
		{
			// Inject current item into page.
			IContentTemplate template = HttpContext.Current.Handler as IContentTemplate;
			if (template != null)
				template.CurrentItem = Zeus.Context.Current.Resolve<IWebContext>().CurrentPage;
		}

		private void context_EndRequest(object sender, EventArgs e)
		{
			ISession session = HttpContext.Current.Items["OpenSession"] as ISession;
			if (session != null)
				session.Dispose();
		}

		public void Dispose()
		{

		}
	}
}
