using System.ComponentModel.DataAnnotations;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class PostFormViewModel
	{
		[Required]
		public string Subject { get; set; }

		[Required]
		public string Message { get; set; }
	}
}