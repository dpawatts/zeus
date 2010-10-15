using System.Web.Mvc;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;
using Zeus.Examples.MinimalMvcExample.ContentTypes;
using Zeus.Examples.MinimalMvcExample.ViewModels;

namespace Zeus.Examples.MinimalMvcExample.Controllers
{
    [Controls(typeof(CustomUrlContainer), AreaName = WebsiteAreaRegistration.AREA_NAME)]
    public class CustomUrlContainerController : ZeusController<CustomUrlContainer>
    {
        public override ActionResult Index()
        {
            return View(new CustomUrlContainerViewModel(CurrentItem));
        }
    }
}
