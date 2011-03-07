using System.ComponentModel;

namespace Zeus.Web.Security
{
	public enum UserCreateStatus
	{
		Success,

		[Description("Invalid username")]
		InvalidUserName,

		[Description("Invalid password")]
		InvalidPassword,

		[Description("Username already exists")]
		DuplicateUserName,

		[Description("User with this email address already exists")]
		DuplicateEmail
	}
}