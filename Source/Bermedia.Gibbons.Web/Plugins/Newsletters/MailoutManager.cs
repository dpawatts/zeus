using System;
using Quiksoft.EasyMail.SMTP;
using Bermedia.Gibbons.Web.Items;
using System.Configuration;
using System.IO;

namespace Bermedia.Gibbons.Web.Plugins.Newsletters
{
	public static class MailoutManager
	{
		public static void SetLicenseKey()
		{
			License.Key = ConfigurationManager.AppSettings["EasyMailLicenseKey"];
		}

		public static EmailMessage GetEmailMessage(Newsletter newsletter)
		{
			// Create the message object. This will be same template for all recipients.
			// Any field can contain a merge token.
			EmailMessage email = new EmailMessage();

			email.From.Email = ConfigurationManager.AppSettings["NewsletterSender"];
			email.Subject = newsletter.Title;

			// Add HTML versions.
			email.BodyParts.Add(new BodyPart(newsletter.MailBodyHtml, BodyPartFormat.HTML));

			// Add attachments.
			foreach (NewsletterAttachment attachment in newsletter.Attachments)
				email.Attachments.Add(new MemoryStream(attachment.File.Data), attachment.File.Name);

			return email;
		}

		public static void Send(Action<SMTP> callback)
		{
			SMTPServer smtpServer = new SMTPServer(ConfigurationManager.AppSettings["SmtpServerName"]);
			if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SmtpServerAccount"]) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["SmtpServerPassword"]))
			{
				smtpServer.AuthMode = SMTPAuthMode.AuthLogin;
				smtpServer.Account = ConfigurationManager.AppSettings["SmtpServerAccount"];
				smtpServer.Password = ConfigurationManager.AppSettings["SmtpServerPassword"];
			}

			using (SMTP smtp = new SMTP())
			{
				smtp.SMTPServers.Add(smtpServer);
				callback(smtp);
			}
		}
	}
}
