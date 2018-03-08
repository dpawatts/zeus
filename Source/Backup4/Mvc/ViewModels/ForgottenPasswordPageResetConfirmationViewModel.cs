using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class ForgottenPasswordPageResetConfirmationViewModel : ViewModel<ForgottenPasswordPage>
	{
		public bool Successful { get; set; }

		public ForgottenPasswordPageResetConfirmationViewModel(ForgottenPasswordPage currentItem, bool successful)
			: base(currentItem)
		{
			Successful = successful;
		}
	}
}


