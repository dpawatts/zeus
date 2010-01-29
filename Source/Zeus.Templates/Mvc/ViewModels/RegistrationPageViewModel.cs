using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class RegistrationPageViewModel : ViewModel<RegistrationPageBase>
	{
		public string CaptchaError { get; set; }

		public RegistrationPageViewModel(RegistrationPageBase currentItem)
			: base(currentItem)
		{
			
		}
	}
}


