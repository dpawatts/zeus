using System;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Registration Page")]
	[RestrictParents(typeof(ILoginContext))]
	public class RegistrationPage : PageContentItem
	{
		[ContentProperty("Verification Email Sender", 100, Description="Email address which should be used as the sender email for the verification email which will be sent to newly registered users.")]
		public virtual string VerificationEmailSender
		{
			get { return GetDetail("VerificationEmailSender", string.Empty); }
			set { SetDetail("VerificationEmailSender", value); }
		}

		[ContentProperty("Verification Email Subject", 110, Description = "Subject line of the verification email which will be sent to newly registered users.")]
		public virtual string VerificationEmailSubject
		{
			get { return GetDetail("VerificationEmailSubject", "Please verify your email address"); }
			set { SetDetail("VerificationEmailSubject", value); }
		}

		[ContentProperty("Verification Email Body", 120, Description = "Body text of the verification email which will be sent to newly registered users. This text MUST contain the text \"{VERIFICATIONLINK}\".")]
		[TextBoxEditor(TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine)]
		public virtual string VerificationEmailBody
		{
			get { return GetDetail("VerificationEmailBody", "Please click the following link to verify your email address:" + Environment.NewLine + "{VERIFICATIONLINK}"); }
			set { SetDetail("VerificationEmailBody", value); }
		}
	}
}