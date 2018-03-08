using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class ForgottenPasswordPageRequestResetViewModel : ViewModel<ForgottenPasswordPage>
	{
		public string Status { get; set; }

		public ForgottenPasswordPageRequestResetViewModel(ForgottenPasswordPage currentItem, string status)
			: base(currentItem)
		{
			Status = status;
		}
	}
}


