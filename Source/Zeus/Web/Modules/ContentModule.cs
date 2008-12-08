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

			// Check if url matches one of our "managed" pages.
			string url = CleanUrl(HttpContext.Current.Request.Path);

			ContentItem currentItem = null;
			if (HttpContext.Current.Request.QueryString["page"] != null)
			{
				currentItem = Context.Persister.Get(HttpContext.Current.Request.GetRequiredInt("page"));
			}
			else
			{
				string[] splitUrl = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
				currentItem = Context.Persister.Load(Context.Current.Host.RootItemID);
				if (currentItem != null)
				{
					for (int i = 0, length = splitUrl.Length; i < length; i++)
					{
						ContentItem childItem = currentItem.Children.SingleOrDefault(c => c.Name.ToLower() == splitUrl[i].ToLower());
						if (childItem != null)
							currentItem = childItem;
						else
							break;
					}
				}
			}

			// If url matches a physical page, don't rewrite.
			if (!File.Exists(HttpContext.Current.Request.PhysicalPath))
			{
				// Rewrite URL.
				HttpContext.Current.RewritePath(currentItem.TemplateUrl + HttpContext.Current.Request.Url.Query);
			}

			HttpContext.Current.Items["CurrentPage"] = currentItem;
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
				template.CurrentItem = (ContentItem) HttpContext.Current.Items["CurrentPage"];
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
