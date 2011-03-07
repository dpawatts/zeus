namespace Zeus.Web.Security
{
	public enum PasswordResetRequestValidity
	{
		NoMatchingRequest,
		AlreadyUsed,
		Expired,
		Valid
	}
}