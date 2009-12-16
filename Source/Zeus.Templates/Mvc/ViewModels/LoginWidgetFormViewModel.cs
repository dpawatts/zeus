using System.ComponentModel.DataAnnotations;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class LoginWidgetFormViewModel
	{
		[Required]
		public string Username { get; set; }

		[DataType(DataType.Password)]
		[Required]
		public string Password { get; set; }
	}
}