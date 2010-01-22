using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Zeus.Web.Mvc;

namespace Zeus.Web.Caching
{
	public class PageCacheFilterAttribute : ActionFilterAttribute
	{
		private readonly ICachingService _cachingService;
		private ContentItem _currentItem;
		private Stream _originalOutputStream;

		public PageCacheFilterAttribute()
		{
			_cachingService = Context.Current.Resolve<ICachingService>();
			Order = 0;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			_currentItem =
				filterContext.Controller.ControllerContext.RouteData.Values[ContentRoute.ContentItemKey] as ContentItem;
			if (_currentItem != null && _cachingService.IsPageCached(_currentItem))
			{
				string cachedHtml = _cachingService.GetCachedPage(_currentItem);
				filterContext.Result = new ContentResult { Content = cachedHtml };
				return;
			}

			if (_currentItem != null && _currentItem.GetPageCachingEnabled())
			{
				_originalOutputStream = filterContext.HttpContext.Response.Filter;
				HttpResponseBase response = filterContext.HttpContext.Response;
				response.Flush();
				response.Filter = new CapturingResponseFilter(response.Filter);
			}
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			if (_originalOutputStream != null)
			{
				HttpResponseBase response = filterContext.HttpContext.Response;
				response.Flush();
				CapturingResponseFilter capturingResponseFilter = (CapturingResponseFilter) filterContext.HttpContext.Response.Filter;
				response.Filter = _originalOutputStream;
				string html = capturingResponseFilter.GetContents(filterContext.HttpContext.Response.ContentEncoding);
				response.Write(html);

				_cachingService.InsertCachedPage(_currentItem, html);
			}
		}

		/// <summary>
		/// CapturingResponseFilter borrowed from MVC Contrib. See the original at http://mvccontrib.googlecode.com/svn/trunk/src/MVCContrib/UI/CapturingResponseFilter.cs
		/// </summary>
		private class CapturingResponseFilter : Stream
		{
			private Stream _sink;
			private MemoryStream mem;

			public CapturingResponseFilter(Stream sink)
			{
				_sink = sink;
				mem = new MemoryStream();
			}

			// The following members of Stream must be overriden.
			public override bool CanRead
			{
				get { return true; }
			}

			public override bool CanSeek
			{
				get { return false; }
			}

			public override bool CanWrite
			{
				get { return false; }
			}

			public override long Length
			{
				get { return 0; }
			}

			public override long Position { get; set; }

			public override long Seek(long offset, SeekOrigin direction)
			{
				return 0;
			}

			public override void SetLength(long length)
			{
				_sink.SetLength(length);
			}

			public override void Close()
			{
				_sink.Close();
				mem.Close();
			}

			public override void Flush()
			{
				_sink.Flush();
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				return _sink.Read(buffer, offset, count);
			}

			// Override the Write method to filter Response to a file. 
			public override void Write(byte[] buffer, int offset, int count)
			{
				//Here we will not write to the sink b/c we want to capture

				//Write out the response to the file.
				mem.Write(buffer, 0, count);
			}

			public string GetContents(Encoding enc)
			{
				var buffer = new byte[mem.Length];
				mem.Position = 0;
				mem.Read(buffer, 0, buffer.Length);
				return enc.GetString(buffer, 0, buffer.Length);
			}
		}
	}
}