using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class RegistrationPageViewModel : ViewModel<RegistrationPage>
	{
		public string CaptchaError { get; set; }

		public RegistrationPageViewModel(RegistrationPage currentItem)
			: base(currentItem)
		{
			
		}
	}
}


