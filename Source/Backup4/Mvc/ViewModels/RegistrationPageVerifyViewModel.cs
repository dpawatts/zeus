using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class RegistrationPageVerifyViewModel : ViewModel<RegistrationPageBase>
	{
		public bool Successful { get; set; }
		public string ErrorMessage { get; set; }

		public RegistrationPageVerifyViewModel(RegistrationPageBase currentItem,
			bool successful, string errorMessage)
			: base(currentItem)
		{
			Successful = successful;
			ErrorMessage = errorMessage;
		}
	}
}