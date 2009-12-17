using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class CommentFormViewModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Email { get; set; }

		[DisplayName("URL")]
		[DataType(DataType.Url)]
		public string Url { get; set; }

		[DataType(DataType.MultilineText)]
		[Required]
		public string Text { get; set; }
	}
}