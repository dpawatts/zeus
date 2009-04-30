using System.Linq;
using System.Web;
using Castle.Core;
using Quiksoft.EasyMail.SMTP;
using Zeus.AddIns.Mailouts.ContentTypes;
using Zeus.AddIns.Mailouts.ContentTypes.ListFilters;
using Zeus.Persistence;
using Zeus.Web;

namespace Zeus.AddIns.Mailouts.Services
{
	public class MailoutService : IMailoutService, IStartable
	{
		private readonly IPersister _persister;

		public MailoutService(IPersister persister)
		{
			_persister = persister;
		}

		public IQueryable<IMailoutRecipient> GetRecipients(Campaign campaign)
		{
			//IQueryable<IMailoutRecipient> recipients = Context.Current.Resolve<IMailoutRecipientService>().GetRecipients();
			IQueryable<IMailoutRecipient> recipients = campaign.List.Recipients.AsQueryable();
			foreach (ListFilter listFilter in campaign.ListFilters)
				recipients = recipients.Where(r => listFilter.Matches(r));
			return recipients;
		}

		public void Send(Campaign campaign)
		{
			License.Key = campaign.MailoutsPlugin.EasyMailLicenseKey;

			// Do the send.
			campaign.Status = CampaignStatus.InProgress;
			Context.Persister.Save(campaign);

			SMTPServer smtpServer = new SMTPServer(campaign.MailoutsPlugin.SmtpServerName);
			if (!string.IsNullOrEmpty(campaign.MailoutsPlugin.SmtpServerAccount) && !string.IsNullOrEmpty(campaign.MailoutsPlugin.SmtpServerPassword))
			{
				smtpServer.AuthMode = SMTPAuthMode.AuthLogin;
				smtpServer.Account = campaign.MailoutsPlugin.SmtpServerAccount;
				smtpServer.Password = campaign.MailoutsPlugin.SmtpServerPassword;
			}

			using (SMTP smtp = new SMTP())
			{
				smtp.SMTPServers.Add(smtpServer);

				IQueryable<IMailoutRecipient> recipients = GetRecipients(campaign);
				foreach (IMailoutRecipient mailoutRecipient in recipients)
				{
					EmailMessage emailMessage = GetEmailMessage(campaign, mailoutRecipient);
					try
					{
						smtp.Send(emailMessage);
					}
					catch (SMTPAuthenticationException ex)
					{
						ProcessError(campaign, mailoutRecipient.Email, "Authentication - " + ex.ErrorCode);
					}
					catch (SMTPConnectionException ex)
					{
						ProcessError(campaign, mailoutRecipient.Email, "Connection - " + ex.ErrorCode);
					}
					catch (SMTPProtocolException ex)
					{
						ProcessError(campaign, mailoutRecipient.Email, "Protocol - " + ex.ErrorCode);
					}
					finally
					{
						++campaign.Emails;
					}
				}
			}

			campaign.Status = CampaignStatus.Sent;
			Context.Persister.Save(campaign);
		}

		private static void ProcessError(Campaign campaign, string email, string error)
		{
			campaign.Children.Add(new RecipientBounce {Parent = campaign, Email = email, Error = error});
		}

		private static EmailMessage GetEmailMessage(Campaign campaign, IMailoutRecipient mailoutRecipient)
		{
			// Create the message object. This will be same template for all recipients.
			// Any field can contain a merge token.
			EmailMessage email = new EmailMessage();

			email.From.Name = campaign.FromName;
			email.From.Email = campaign.FromEmail;
			email.ReplyTo = campaign.ReplyToEmail;
			email.Subject = campaign.MessageSubject;
			email.Recipients.Add(mailoutRecipient.Email);

			email.BodyParts.Add(new BodyPart(ReplacePlaceHolders(campaign, mailoutRecipient, campaign.HtmlMessage), BodyPartFormat.HTML));
			email.BodyParts.Add(new BodyPart(ReplacePlaceHolders(campaign, mailoutRecipient, campaign.PlainTextMessage), BodyPartFormat.Plain));

			// Add attachments.
			//foreach (NewsletterAttachment attachment in newsletter.Attachments)
			//	email.Attachments.Add(new MemoryStream(attachment.File.Data), attachment.File.Name);

			return email;
		}

		private static string ReplacePlaceHolders(Campaign campaign, IMailoutRecipient mailoutRecipient, string message)
		{
			string unsubscribeLink = Context.Current.WebContext.Url
				.SetPath(campaign.List.UnsubscribePage.Url)
				.SetQuery(null).SetQueryParameter("e", mailoutRecipient.Email)
				.ToString();
			message = message.Replace("*|UNSUB|*", unsubscribeLink);
			message = message.Replace("*%7CUNSUB%7C*", unsubscribeLink);
			return message;
		}

		public void Start()
		{
			_persister.ItemCopied += ItemCopiedEvenHandler;
		}

		public void Stop()
		{
			_persister.ItemCopied -= ItemCopiedEvenHandler;
		}

		private void ItemCopiedEvenHandler(object sender, DestinationEventArgs e)
		{
			if (e.AffectedItem is Campaign)
			{
				Campaign campaign = (Campaign) e.AffectedItem;
				campaign.Status = CampaignStatus.Unsent;
				foreach (RecipientBounce bounce in campaign.Children.OfType<RecipientBounce>())
					campaign.Children.Remove(bounce);
				_persister.Save(campaign);
			}
		}
	}
}