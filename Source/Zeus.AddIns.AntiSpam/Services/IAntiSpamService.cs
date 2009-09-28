namespace Zeus.AddIns.AntiSpam.Services
{
	/// <summary>
	/// Interface for anti-spam services, such as RECAPTCHA, Waegis or Akismet.
	/// </summary>
	public interface IAntiSpamService
	{
		/// <summary>
		/// Implemented by each anti-spam service to do the actual check.
		/// </summary>
		/// <returns></returns>
		SpamStatus Check();
	}
}