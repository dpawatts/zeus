using System.Web.UI;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.AddIns.Mailouts.ContentTypes
{
	[ContentType("Mailouts Plugin")]
	[ContentTypeAuthorizedRoles("Administrator")]
	[RestrictParents(typeof(IRootItem))]
	public class MailoutsPlugin : ContentItem
	{
		public MailoutsPlugin()
		{
			Title = "Mailouts";
			Name = "mailouts";
		}

		public override string IconUrl
		{
			get { return new Page().ClientScript.GetWebResourceUrl(typeof(MailoutsPlugin), "Zeus.AddIns.Mailouts.Resources.email_open.png"); }
		}

		[TextBoxEditor("EasyMail License Key", 1, Required = true)]
		[PropertyAuthorizedRoles("Administrator")]
		public virtual string EasyMailLicenseKey
		{
			get { return GetDetail("EasyMailLicenseKey", string.Empty); }
			set { SetDetail("EasyMailLicenseKey", value); }
		}

		[TextBoxEditor("SMTP Server Name", 2, Description = "Name or address of server", Required = true)]
		public virtual string SmtpServerName
		{
			get { return GetDetail("SmtpServerName", string.Empty); }
			set { SetDetail("SmtpServerName", value); }
		}

		[TextBoxEditor("SMTP Server Account", 4, Description = "Gets or sets the name of the user authentication account.")]
		public virtual string SmtpServerAccount
		{
			get { return GetDetail("SmtpServerAccount", string.Empty); }
			set { SetDetail("SmtpServerAccount", value); }
		}

		[TextBoxEditor("SMTP Server Password", 6, Description = "Gets or sets the password of the authentication account.")]
		public virtual string SmtpServerPassword
		{
			get { return GetDetail("SmtpServerPassword", string.Empty); }
			set { SetDetail("SmtpServerPassword", value); }
		}
	}
}