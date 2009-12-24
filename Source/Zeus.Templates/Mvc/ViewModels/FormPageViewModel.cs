using Zeus.Templates.ContentTypes.Forms;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class FormPageViewModel : ViewModel<FormPage>
	{
		public FormPageViewModel(FormPage currentItem)
			: base(currentItem)
		{
			
		}
	}
}