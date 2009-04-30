using System.Collections.Generic;
using System.Web.UI;
using Zeus.AddIns.Mailouts.ContentTypes.FormFields;
using Zeus.AddIns.Mailouts.Services;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Web.UI;

namespace Zeus.AddIns.Mailouts.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(MailoutsPlugin))]
	[FieldSet("ListName", "List Name", 10)]
	[FieldSet("CampaignDefaults", "Campaign Defaults", 20)]
	[FieldSet("Unsubscribe", "Unsubscribe", 30)]
	[FieldSet("FormFieldsContainer", "Form Fields", 40)]
	public class List : ContentItem
	{
		public override string IconUrl
		{
			get { return new Page().ClientScript.GetWebResourceUrl(typeof(List), "Zeus.AddIns.Mailouts.Resources.group.png"); }
		}

		[TextBoxEditor("Name", 10, Required = true, ContainerName = "ListName", Description = "Good example: 'Acme Company Newsletter' - Bad example: 'Cust_11_01_2007'")]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[TextBoxEditor("Default \"From Name\"", 20, ContainerName = "CampaignDefaults", Description = "Use something instantly recognizable (like your company name)", Required = true)]
		public virtual string DefaultFromName
		{
			get { return GetDetail("DefaultFromName", string.Empty); }
			set { SetDetail("DefaultFromName", value); }
		}

		[TextBoxEditor("Default \"From Email\"", 30, ContainerName = "CampaignDefaults", Description = "The email will be sent from this email address", Required = true)]
		public virtual string DefaultFromEmail
		{
			get { return GetDetail("DefaultFromEmail", string.Empty); }
			set { SetDetail("DefaultFromEmail", value); }
		}

		[TextBoxEditor("Default \"Reply-to Email\"", 40, ContainerName = "CampaignDefaults", Description = "Make sure someone actually checks this email account", Required = true)]
		public virtual string DefaultReplyToEmail
		{
			get { return GetDetail("DefaultReplyToEmail", string.Empty); }
			set { SetDetail("DefaultReplyToEmail", value); }
		}

		[TextBoxEditor("Default \"Subject Line\"", 50, ContainerName = "CampaignDefaults", Description = "Keep it relevant and non-spammy")]
		public virtual string DefaultSubjectLine
		{
			get { return GetDetail("DefaultSubjectLine", string.Empty); }
			set { SetDetail("DefaultSubjectLine", value); }
		}

		[LinkedItemDropDownListEditor("Unsubscribe Page", 60, Required = true, TypeFilter = typeof(ContentItem), ContainerName = "Unsubscribe")]
		public virtual ContentItem UnsubscribePage
		{
			get { return GetDetail<ContentItem>("UnsubscribePage", null); }
			set { SetDetail("UnsubscribePage", value); }
		}

		[ChildrenEditor("Form Fields", 70, AddNewText = "Add New Form Field", TypeFilter = typeof(FormField), ContainerName = "FormFieldsContainer")]
		public virtual IList<FormField> FormFields
		{
			get { return GetChildren<FormField>(); }
		}

		public virtual PropertyCollection InterestGroups
		{
			get { return GetDetailCollection("InterestGroups", true); }
		}

		public virtual IList<IMailoutRecipient> Recipients
		{
			get { return GetChildren<IMailoutRecipient>(); }
		}
	}
}