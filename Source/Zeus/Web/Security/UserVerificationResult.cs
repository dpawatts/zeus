using System.ComponentModel;

namespace Zeus.Web.Security
{
	public enum UserVerificationResult
	{
		Verified,

		[Description("No matching user")]
		NoMatchingUser,

		[Description("Already verified")]
		AlreadyVerified
	}
}