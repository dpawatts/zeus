using System.Web.Mvc;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;
using Zeus.Examples.MinimalMvcExample.ContentTypes;
using Zeus.Examples.MinimalMvcExample.ViewModels;

namespace Zeus.Examples.MinimalMvcExample.Controllers
{
    [Controls(typeof(CustomUrlPage), AreaName = WebsiteAreaRegistration.AREA_NAME)]
    public class CustomUrlPageController : ZeusController<CustomUrlPage>
    {
        public override ActionResult Index()
        {
            return View(new CustomUrlPageViewModel(CurrentItem));
        }
    }
}
