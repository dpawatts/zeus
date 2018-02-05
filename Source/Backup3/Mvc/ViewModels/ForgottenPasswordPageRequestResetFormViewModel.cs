using System.ComponentModel.DataAnnotations;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class ForgottenPasswordPageRequestResetFormViewModel
	{
		[Required]
		public string Username { get; set; }
	}
}