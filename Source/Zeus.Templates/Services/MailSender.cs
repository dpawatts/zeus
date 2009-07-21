using System.Net.Mail;
using Zeus.Net.Mail;
using Zeus.Persistence;
using Zeus.Web;

namespace Zeus.Templates.Services
{
	/// <summary>
	/// Sends the mail message through the smtp server configured in <system.net>.
	/// </summary>
	public class MailSender : SmtpMailSender
	{
		protected override SmtpClient GetSmtpClient()
		{
			return CreateSmtpClient(null, -1, null, null);
		}
	}
}