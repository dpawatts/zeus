using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.FileSystem;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Title = "Newsletter Log Entry")]
	[RestrictParents(typeof(NewsletterSubscription))]
	public class NewsletterLogEntry : BaseContentItem
	{
		public Newsletter Newsletter
		{
			get { return GetDetail<Newsletter>("Newsletter", null); }
			set { SetDetail<Newsletter>("Newsletter", value); }
		}

		public NewsletterLogEntryStatus Status
		{
			get { return GetDetail<NewsletterLogEntryStatus>("Status", NewsletterLogEntryStatus.NotAttempted); }
			set { SetDetail<NewsletterLogEntryStatus>("Status", value); }
		}

		public string ErrorMessage
		{
			get { return GetDetail<string>("ErrorMessage", null); }
			set { SetDetail<string>("ErrorMessage", value); }
		}

		protected override string IconName
		{
			get { return "newspaper"; }
		}
	}
}
