using System.Web.Mvc;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;
using Zeus.Examples.MinimalMvcExample.ContentTypes;
using Zeus.Examples.MinimalMvcExample.ViewModels;

namespace Zeus.Examples.MinimalMvcExample.Controllers
{
	[Controls(typeof(MyPage), AreaName = WebsiteAreaRegistration.AREA_NAME)]
	public class MyPageController : ZeusController<MyPage>
	{
        [NonAction]
		public override ActionResult Index()
		{
			return View(new MyPageViewModel(CurrentItem, ""));
		}


        public ActionResult Index(string param)
        {
            return View(new MyPageViewModel(CurrentItem, param));
        }


	}
}
