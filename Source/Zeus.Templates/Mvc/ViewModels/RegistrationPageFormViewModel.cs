using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zeus.Templates.Mvc.ViewModels
{
	[PasswordConfirmMatchesPassword]
	public class RegistrationPageFormViewModel : IConfirmPassword
	{
		[Required]
		public string Username { get; set; }

		[DisplayName("Email Address")]
		[DataType(DataType.EmailAddress)]
		[Required]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		[Required]
		public string Password { get; set; }

		[DisplayName("Confirm Password")]
		[DataType(DataType.Password)]
		[Required]
		public string ConfirmPassword { get; set; }
	}
}