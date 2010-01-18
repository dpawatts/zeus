using System.Web.Mvc;
using Zeus.Templates.ContentTypes.Forms;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(FormPage), AreaName = TemplatesAreaRegistration.AREA_NAME)]
	public class FormPageController : ZeusController<FormPage>
	{
		public override ActionResult Index()
		{
			return View(new FormPageViewModel(CurrentItem));
		}
	}
}