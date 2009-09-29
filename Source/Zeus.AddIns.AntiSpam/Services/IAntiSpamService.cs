namespace Zeus.AddIns.AntiSpam.Services
{
	/// <summary>
	/// Interface for anti-spam services, such as RECAPTCHA, Waegis or Akismet.
	/// </summary>
	public interface IAntiSpamService
	{
		/// <summary>
		/// Implemented by each anti-spam service to do the actual check.
		/// Returns true if the comment is spam, otherwise false.
		/// </summary>
		/// <returns></returns>
		bool CheckCommentForSpam(ContentItem currentStartPage, IAntiSpamComment comment);

		/// <summary>
		/// Submits a comment to Akismet that should have been 
		/// flagged as SPAM, but was not flagged by Akismet.
		/// </summary>
		/// <param name="currentStartPage"></param>
		/// <param name="comment"></param>
		/// <returns></returns>
		void SubmitSpam(ContentItem currentStartPage, IAntiSpamComment comment);

		/// <summary>
		/// Submits a comment to Akismet that should not have been 
		/// flagged as SPAM (a false positive).
		/// </summary>
		/// <param name="currentStartPage"></param>
		/// <param name="comment"></param>
		/// <returns></returns>
		void SubmitHam(ContentItem currentStartPage, IAntiSpamComment comment);

		/// <summary>
		/// Verifies the API key.  You really only need to
		/// call this once, perhaps at startup.
		/// </summary>
		bool VerifyApiKey(ContentItem currentStartPage);
	}
}