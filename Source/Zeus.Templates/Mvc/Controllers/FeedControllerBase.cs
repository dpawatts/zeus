using System;
using System.Collections.Generic;
using System.Globalization;
using Zeus.Templates.Services.Syndication;

namespace Zeus.Templates.Mvc.Controllers
{
	public class FeedControllerBase<T> : ZeusController<T>
		where T : ContentItem, IFeed
	{
		/// <summary>
		/// Returns the "If-None-Match" HTTP header.  This is used to indicate 
		/// a conditional GET and is used to implement RFC3229 with feeds 
		/// <see href="http://wyman.us/main/2004/09/using_rfc3229_w.html"/>.
		/// </summary>
		/// <value></value>
		protected string IfNoneMatchHeader
		{
			get { return HttpContext.Request.Headers["If-None-Match"]; }
		}

		/// <summary>
		/// Gets the Publish Date of the last feed item received by the client. 
		/// This is used to determine whether the client is up to date 
		/// or whether the client is ready to receive new feed items. 
		/// We will then send just the difference.
		/// </summary>
		/// <value></value>
		protected DateTime PublishDateOfLastFeedItemReceived
		{
			get
			{
				if (!string.IsNullOrEmpty(IfNoneMatchHeader))
				{
					try
					{
						return DateTime.Parse(IfNoneMatchHeader, CultureInfo.InvariantCulture);
					}
					catch (FormatException)
					{
						//Swallow it.
					}
				}
				return DateTime.MinValue;
			}
		}

		/// <summary>
		/// Gets a value indicating whether use delta encoding within this request.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if use delta encoding; otherwise, <c>false</c>.
		/// </value>
		protected bool UseDeltaEncoding
		{
			get { return CurrentItem.Rfc3229DeltaEncodingEnabled && AcceptDeltaEncoding; }
		}

		/// <summary>
		/// Gets the accept IM header from the request.
		/// </summary>
		/// <value></value>
		protected string AcceptImHeader
		{
			get { return HttpContext.Request.Headers["A-IM"] ?? string.Empty; }
		}

		/// <summary>
		/// Gets a value indicating whether the client accepts 
		/// <see href="http://wyman.us/main/2004/09/using_rfc3229_w.html">RFC3229 Feed Delta 
		/// Encoding</see>. 
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [accepts delta encoding]; otherwise, <c>false</c>.
		/// </value>
		protected bool AcceptDeltaEncoding
		{
			get { return AcceptImHeader.IndexOf("feed") >= 0; }
		}

		protected Feed GetFeed()
		{
			DateTime latestPublishDate = PublishDateOfLastFeedItemReceived;
			bool clientHasAllFeedItems = true;
			List<ISyndicatable> items = new List<ISyndicatable>();
			foreach (ISyndicatable syndicatable in CurrentItem.GetItems())
			{
				if (UseDeltaEncoding && syndicatable.Published <= PublishDateOfLastFeedItemReceived)
				{
					// Since Entries are ordered by DatePublished descending, as soon 
					// as we encounter one that is smaller than or equal to 
					// one the client has already seen, we're done as we 
					// know the client already has the rest of the items in 
					// the collection.
					break;
				}

				items.Add(syndicatable);

				if (syndicatable.Published > latestPublishDate)
					latestPublishDate = syndicatable.Published;

				clientHasAllFeedItems = false;
			}

			return new Feed(items, UseDeltaEncoding, latestPublishDate, clientHasAllFeedItems, CurrentItem.Title,
				CurrentItem.Url, CurrentItem.Tagline, CurrentItem.Author, CurrentItem.Published ?? DateTime.Now);
		}
	}
}