using System;
using System.Globalization;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Zeus.Templates.Mvc
{
	public abstract class SyndicationResultBase : ActionResult
	{
		private const int HttpImUsed = 226;

		protected abstract string ContentType { get; }

		protected Feed Feed { get; private set; }

		protected HttpContextBase HttpContext
		{
			get { return Context.Current.WebContext.HttpContext; }
		}

		/// <summary>
		/// Returns the "If-Modified-Since" HTTP header.  This indicates 
		/// the last time the client requested data and is used to 
		/// determine whether new data is to be sent.
		/// </summary>
		/// <value></value>
		protected string LastModifiedHeader
		{
			get { return HttpContext.Request.Headers["If-Modified-Since"]; }
		}

		/// <summary>
		/// Returns the "Accept-Encoding" value from the HTTP Request header. 
		/// This is a list of encodings that may be sent to the browser.
		/// </summary>
		/// <remarks>
		/// Specifically we're looking for gzip.
		/// </remarks>
		/// <value></value>
		protected string AcceptEncoding
		{
			get { return HttpContext.Request.Headers["Accept-Encoding"] ?? string.Empty; }
		}

		protected SyndicationResultBase(Feed feed)
		{
			Feed = feed;
		}

		/// <summary>
		/// Compares the requesting clients <see cref="LastModifiedHeader"/> against 
		/// the date the feed was last updated.  If the feed hasn't been updated, then 
		/// it sends a 304 HTTP header indicating such.
		/// </summary>
		/// <returns></returns>
		protected virtual bool IsLocalCacheOk()
		{
			string dt = LastModifiedHeader;
			if (dt != null)
			{
				try
				{
					DateTime feedDt = DateTime.Parse(dt, CultureInfo.InvariantCulture);
					DateTime lastUpdated = Feed.LastModified;
					TimeSpan ts = feedDt - lastUpdated;

					// We need to allow some margin of error.
					return Math.Abs(ts.TotalMilliseconds) <= 500;
				}
				catch (FormatException)
				{
					// TODO: Review
					// Swallow it for now.
					// Some browsers send a funky last modified header.
					// We don't want to throw an exception in those cases.
				}
			}
			return false;
		}

		/// <summary>
		/// Send the HTTP status code 304 to the response this instance.
		/// </summary>
		private void Send304()
		{
			HttpContext.Response.StatusCode = 304;
		}

		public sealed override void ExecuteResult(ControllerContext context)
		{
			// Checks Last Modified Header.
			if (IsLocalCacheOk())
			{
				Send304();
				return;
			}

			if (Feed.UseDeltaEncoding && Feed.ClientHasAllFeedItems)
			{
				Send304();
				return;
			}

			context.HttpContext.Response.ContentType = ContentType;
			context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
			context.HttpContext.Response.Cache.SetLastModified(Feed.LastModified);
			context.HttpContext.Response.Cache.SetETag(Feed.LastModified.ToString(CultureInfo.InvariantCulture));

			context.HttpContext.Response.AddHeader("IM", "feed");
			if (Feed.UseDeltaEncoding)
				HttpContext.Response.StatusCode = HttpImUsed; //IM Used
			else
				HttpContext.Response.StatusCode = (int) HttpStatusCode.OK;

			WriteXml(context);
		}

		protected abstract void WriteXml(ControllerContext context);
	}
}