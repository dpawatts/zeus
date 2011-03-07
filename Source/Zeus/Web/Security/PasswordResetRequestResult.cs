namespace Zeus.Web.Security
{
	public enum PasswordResetRequestResult
	{
		RequestExists,
		TooManyRequests,
		Sent,
		UserNotFound
	}
}