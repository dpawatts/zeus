namespace Zeus.Templates.Services.AntiSpam
{
	public class CaptchaException : ZeusException
	{
		public CaptchaException(string message) : base(message)
		{
		}

		public CaptchaException(string message, string captchaError)
			: base(message)
		{
			CaptchaError = captchaError;
		}

		public string CaptchaError { get; set; }
	}
}