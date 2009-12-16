using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class RegistrationPageVerifyViewModel : ViewModel<RegistrationPage>
	{
		public bool Successful { get; set; }
		public string ErrorMessage { get; set; }

		public RegistrationPageVerifyViewModel(RegistrationPage currentItem,
			bool successful, string errorMessage)
			: base(currentItem)
		{
			Successful = successful;
			ErrorMessage = errorMessage;
		}
	}
}


