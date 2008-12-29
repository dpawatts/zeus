using System;
using System.Linq;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.FileSystem;
using Zeus.Web.UI;
using System.Collections.Generic;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "Newsletter")]
	[RestrictParents(typeof(NewsletterContainer))]
	[TabPanel(Tabs.General, "General", 1)]
	[TabPanel("Attachments", "Attachments", 2)]
	public class Newsletter : BaseContentItem
	{
		[LiteralDisplayer(Title = "Subject")]
		[TextBoxEditor("Subject", 10, Required = true, ContainerName = Tabs.General)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[DateDisplayer(Title = "Date")]
		[DateEditor("Date", 20, ContainerName = Tabs.General)]
		public override DateTime Created
		{
			get { return base.Created; }
			set { base.Created = value; }
		}

		[HtmlTextBoxEditor("Mail Body (HTML)", 30, ContainerName = Tabs.General)]
		public string MailBodyHtml
		{
			get { return GetDetail<string>("MailBodyHtml", string.Empty); }
			set { SetDetail<string>("MailBodyHtml", value); }
		}

		[ChildrenEditor("Attachments", 40, TypeFilter = typeof(NewsletterAttachment), ContainerName = "Attachments")]
		public IList<NewsletterAttachment> Attachments
		{
			get { return GetChildren<NewsletterAttachment>(); }
		}

		public int TotalMessages
		{
			get { return GetDetail<int>("TotalMessages", 0); }
			set { SetDetail<int>("TotalMessages", value); }
		}

		public int CurrentMessage
		{
			get { return GetDetail<int>("CurrentMessage", 0); }
			set { SetDetail<int>("CurrentMessage", value); }
		}

		[LiteralDisplayer]
		public NewsletterStatus Status
		{
			get { return GetDetail<NewsletterStatus>("Status", NewsletterStatus.NotStarted); }
			set { SetDetail<NewsletterStatus>("Status", value); }
		}

		public string ErrorMessage
		{
			get { return GetDetail<string>("ErrorMessage", null); }
			set { SetDetail<string>("ErrorMessage", value); }
		}

		public IQueryable<NewsletterLogEntry> LogEntries
		{
			get { return Zeus.Context.Current.Finder.OfType<NewsletterLogEntry>().ToList().Where(nle => nle.Newsletter == this).AsQueryable(); }
		}

		protected override string IconName
		{
			get { return "newspaper"; }
		}
	}
}
