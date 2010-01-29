using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class RegistrationPageConfirmationViewModel : ViewModel<RegistrationPageBase>
	{
		public bool EmailVerificationRequired { get; set; }
		public string Email { get; set; }

		public RegistrationPageConfirmationViewModel(RegistrationPageBase currentItem,
			bool emailVerificationRequired, string email)
			: base(currentItem)
		{
			EmailVerificationRequired = emailVerificationRequired;
			Email = email;
		}
	}
}


