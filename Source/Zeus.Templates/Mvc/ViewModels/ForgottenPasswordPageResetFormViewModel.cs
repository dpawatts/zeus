using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zeus.Templates.Mvc.ViewModels
{
	[PasswordConfirmMatchesPassword]
	public class ForgottenPasswordPageResetFormViewModel : IConfirmPassword
	{
		[DataType(DataType.Password)]
		[Required]
		public string Password { get; set; }

		[DisplayName("Confirm Password")]
		[DataType(DataType.Password)]
		[Required]
		public string ConfirmPassword { get; set; }
	}
}