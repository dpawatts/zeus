using Zeus.Templates.ContentTypes.Forms;
using System.Web.Mvc;

namespace Zeus.Templates.Mvc.Controllers.Forms
{
    public class FormPageController : BaseController<FormPage, IFormPageViewData>
    {
        public override ActionResult Index()
        {
            return View("Index", TypedViewData);
        }
    }

    public interface IFormPageViewData : IViewData<FormPage>
    {
        
    }
}