using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Forgotten Password Page")]
	[RestrictParents(typeof(ILoginContext))]
	public class ForgottenPasswordPage : BasePage
	{
		[ContentProperty("Password Reset Email Sender", 100, Description = "Email address which should be used as the sender email for the email which will be sent to users who want to reset their password.")]
		public virtual string PasswordResetEmailSender
		{
			get { return GetDetail("PasswordResetEmailSender", string.Empty); }
			set { SetDetail("PasswordResetEmailSender", value); }
		}

		[ContentProperty("Password Reset Email Subject", 110, Description = "Subject line of the email which will be sent to users who want to reset their password.")]
		public virtual string PasswordResetEmailSubject
		{
			get { return GetDetail("PasswordResetEmailSubject", "Password Reset Request"); }
			set { SetDetail("PasswordResetEmailSubject", value); }
		}

		[ContentProperty("Password Reset Email Body", 120, Description = "Body text of the email which will be sent to users who want to reset their password. This text MUST contain the text \"" + CredentialService.PasswordResetLinkName + "\".")]
		[TextBoxEditor(TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine)]
		public virtual string PasswordResetEmailBody
		{
			get
			{
				string defaultValue = string.Format(@"To initiate the password reset process for your
account, click the link below:

{0}

If clicking the link above doesn't work, please copy and paste the URL in a
new browser window instead.

If you've received this mail in error, it's likely that another user entered
your email address by mistake while trying to reset a password. If you didn't
initiate the request, you don't need to take any further action and can safely
disregard this email.", CredentialService.PasswordResetLinkName);
				return GetDetail("VerificationEmailBody", defaultValue); }
			set { SetDetail("VerificationEmailBody", value); }
		}
	}
}