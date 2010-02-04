using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Web.UI;
using ListFilter = Zeus.AddIns.Mailouts.ContentTypes.ListFilters.ListFilter;

namespace Zeus.AddIns.Mailouts.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(MailoutsPlugin))]
	[FieldSet("ListContainer", "List", 10)]
	[FieldSet("General", "General", 20)]
	public class Campaign : ContentItem
	{
		internal MailoutsPlugin MailoutsPlugin
		{
			get { return (MailoutsPlugin) Parent; }
		}

		protected override Icon Icon
		{
			get { return Icon.Email; }
		}

		[LinkedItemDropDownListEditor("List", 10, Required = true, TypeFilter = typeof(List), ContainerName = "ListContainer")]
		public virtual List List
		{
			get { return GetDetail<List>("List", null); }
			set { SetDetail("List", value); }
		}

		[ChildrenEditor("Filters", 15, AddNewText = "Add New Filter", TypeFilter = typeof(ListFilter), ContainerName = "ListContainer")]
		public virtual IEnumerable<ListFilter> ListFilters
		{
			get { return GetChildren<ListFilter>(); }
		}

		[TextBoxEditor("Name (Internal Use)", 10, Description = "For your own reference, for example 'Newsletter Test #1'", Required = true, ContainerName = "General")]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[TextBoxEditor("Message Subject", 20, Description = "Keep it relevant and non-spammy to avoid spam filters.", Required = true, ContainerName = "General")]
		public virtual string MessageSubject
		{
			get { return GetDetail("MessageSubject", (List != null) ? List.DefaultSubjectLine : string.Empty); }
			set { SetDetail("MessageSubject", value); }
		}

		[TextBoxEditor("From Name", 30, Description = "Use something they'll instantly recognize, like your company name.", Required = true, ContainerName = "General")]
		public virtual string FromName
		{
			get { return GetDetail("FromName", (List != null) ? List.DefaultFromName : string.Empty); }
			set { SetDetail("FromName", value); }
		}

		[TextBoxEditor("From Email", 35, Description = "The email will be sent from this email address", Required = true, ContainerName = "General")]
		public virtual string FromEmail
		{
			get { return GetDetail("FromEmail", (List != null) ? List.DefaultFromEmail : string.Empty); }
			set { SetDetail("FromEmail", value); }
		}

		[TextBoxEditor("Reply-to Email", 40, Description = "Their replies will go to this email address.", Required = true, ContainerName = "General")]
		public virtual string ReplyToEmail
		{
			get { return GetDetail("ReplyToEmail", (List != null) ? List.DefaultReplyToEmail : string.Empty); }
			set { SetDetail("ReplyToEmail", value); }
		}

		[HtmlTextBoxEditor("HTML Message", 50, Required = true, DomainAbsoluteUrls = true, ContainerName = "General", Description = "Use &lt;a href=\"*%7CUNSUB%7C*\"&gt;Unsubscribe&lt;/a&gt; to generate an unsubscribe link.")]
		public virtual string HtmlMessage
		{
			get { return GetDetail("HtmlMessage", string.Empty); }
			set { SetDetail("HtmlMessage", value); }
		}

		[TextBoxEditor("Plain-Text Message", 60, Required = true, TextMode = TextBoxMode.MultiLine, Description = "This plain-text email is displayed if recipients can't (or won't) display your HTML email. Your message might get trapped in spam filters without a plain-text message. Use *|UNSUB|* to generate an unsubscribe link.", ContainerName = "General")]
		public virtual string PlainTextMessage
		{
			get { return GetDetail("PlainTextMessage", string.Empty); }
			set { SetDetail("PlainTextMessage", value); }
		}

		public virtual CampaignStatus Status
		{
			get { return GetDetail("Status", CampaignStatus.Unsent); }
			set { SetDetail("Status", value); }
		}

		public virtual int Emails
		{
			get { return GetDetail("Emails", 0); }
			set { SetDetail("Emails", value); }
		}
	}

	public enum CampaignStatus
	{
		Unsent,
		InProgress,
		Sent
	}
}