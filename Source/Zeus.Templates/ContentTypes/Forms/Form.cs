using System.Collections.Generic;
using System.Web.UI.WebControls;
using Ext.Net;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Web.UI;

namespace Zeus.Templates.ContentTypes.Forms
{
	[ContentType("Form", "Form", "A form that can be sumitted and sent to an email address.", "", 250)]
	[AllowedZones("Content", "ColumnLeft", "ColumnRight")]
	[RestrictParents(typeof(FormPage))]
	//[AllowedChildren(typeof(Question))]
	[AvailableZone("Questions", "Questions")]
	public class Form : BaseWidget
	{
		[TextBoxEditor("Form Title", 10)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[XhtmlStringContentProperty("Intro Text", 100)]
		public virtual string IntroText
		{
			get { return GetDetail("IntroText", string.Empty); }
			set { SetDetail("IntroText", value); }
		}

		[ChildrenEditor("Form Fields", 110, TypeFilter = typeof(IQuestion))]
		public virtual IEnumerable<IQuestion> FormFields
		{
			get { return GetChildren<IQuestion>(); }
		}

		[ContentProperty("Mail From", 120)]
		public virtual string MailFrom
		{
			get { return GetDetail("MailFrom", string.Empty); }
			set { SetDetail("MailFrom", value); }
		}

		[ContentProperty("Mail To", 130)]
		public virtual string MailTo
		{
			get { return GetDetail("MailTo", string.Empty); }
			set { SetDetail("MailTo", value); }
		}

		[ContentProperty("Mail Subject", 140)]
		public virtual string MailSubject
		{
			get { return GetDetail("MailSubject", string.Empty); }
			set { SetDetail("MailSubject", value); }
		}

		[ContentProperty("Mail Body", 150)]
		[TextBoxEditor(TextMode = TextBoxMode.MultiLine)]
		public virtual string MailBody
		{
			get { return (string)(GetDetail("MailBody") ?? string.Empty); }
			set { SetDetail("MailBody", value, string.Empty); }
		}

		[XhtmlStringContentProperty("Submit Text", 160)]
		public virtual string SubmitText
		{
			get { return GetDetail("SubmitText", string.Empty); }
			set { SetDetail("SubmitText", value); }
		}

		protected override Icon Icon
		{
			get { return Icon.Report; }
		}
	}
}