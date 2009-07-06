namespace Zeus.Templates.Services
{
	public interface IEmailSender
	{
		void SendEmail(string from, string to, string subject, string body);
	}
}