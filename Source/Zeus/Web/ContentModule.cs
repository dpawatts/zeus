using System;
using System.Linq;
using System.Web;
using System.IO;
using Zeus.Web.UI;

namespace Zeus.Web
{
	public class ContentModule : IHttpModule
	{
		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(context_BeginRequest);
			context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
		}

		private void context_BeginRequest(object sender, EventArgs e)
		{
			// If url matches a physical page, don't rewrite.
			if (File.Exists(HttpContext.Current.Request.PhysicalPath))
				return;

			if (Path.GetExtension(HttpContext.Current.Request.Path) != ".aspx")
				return;

			// Check if url matches one of our "managed" pages.
			string url = CleanUrl(HttpContext.Current.Request.Path);

			string[] splitUrl = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			ContentItem currentItem = Context.Persister.Load(Context.Current.Host.RootItemID);
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

				// Rewrite URL.
				HttpContext.Current.RewritePath(currentItem.TemplateUrl + HttpContext.Current.Request.Url.Query);

				HttpContext.Current.Items["CurrentPage"] = currentItem;
			}
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

		public void Dispose()
		{

		}
	}
}
