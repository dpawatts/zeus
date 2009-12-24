using Zeus.Templates.ContentTypes.Forms;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class FormSubmitViewModel : ViewModel<Form>
	{
		public FormSubmitViewModel(Form currentItem)
			: base(currentItem)
		{
			
		}
	}
}