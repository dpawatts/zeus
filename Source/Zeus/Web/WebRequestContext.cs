using System;
using System.Web;
using Zeus.BaseLibrary.Web;

namespace Zeus.Web
{
	public class WebRequestContext : WebContext, IWebContext, IDisposable
	{
		public ContentItem CurrentPage
		{
			get { return CurrentHttpContext.Items["CurrentPage"] as ContentItem; }
			set { CurrentHttpContext.Items["CurrentPage"] = value; }
		}

		public PathData CurrentPath
		{
			get { return RequestItems["CurrentTemplate"] as PathData; }
			set
			{
				RequestItems["CurrentTemplate"] = value;
				if (value != null)
					CurrentPage = value.CurrentItem as ContentItem;
				else
					CurrentPage = null;
			}
		}

		/// <summary>The physical path on disk to the requested resource.</summary>
		public virtual string PhysicalPath
		{
			get { return Request.PhysicalPath; }
		}

		public HttpContextBase HttpContext
		{
			get { return new HttpContextWrapper(CurrentHttpContext); }
		}

		public virtual void Close()
		{
			object[] keys = new object[RequestItems.Keys.Count];
			RequestItems.Keys.CopyTo(keys, 0);

			foreach (object key in keys)
			{
				IClosable value = RequestItems[key] as IClosable;
				if (value != null)
					value.Dispose();
			}
		}

		public void TransferRequest(string path)
		{
			string url = Url.Parse(path).AppendQuery("postback", Url.LocalUrl);
			CurrentHttpContext.Server.TransferRequest(url, true);
		}

		public string GetFullyQualifiedUrl(string url)
		{
			return Url.HostUrl + url;
		}

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			Close();
		}

		#endregion
	}
}
