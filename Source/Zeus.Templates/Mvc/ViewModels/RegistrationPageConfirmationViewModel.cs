using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class RegistrationPageConfirmationViewModel : ViewModel<RegistrationPage>
	{
		public bool EmailVerificationRequired { get; set; }
		public string Email { get; set; }

		public RegistrationPageConfirmationViewModel(RegistrationPage currentItem,
			bool emailVerificationRequired, string email)
			: base(currentItem)
		{
			EmailVerificationRequired = emailVerificationRequired;
			Email = email;
		}
	}
}


