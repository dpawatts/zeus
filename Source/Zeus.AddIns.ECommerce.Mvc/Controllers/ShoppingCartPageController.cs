using Zeus.AddIns.ECommerce.ContentTypes;
using Zeus.Templates.Mvc.Controllers;
using System.Web.Mvc;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
    [Controls(typeof(ShoppingCartPage))]
    public class ShoppingCartPageController : ZeusController<ShoppingCartPage>
    {
        public override ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(int productID)
        {
            return View();
        }

        public ActionResult UpdateQuantity(int productID, int quantity)
        {
            return View();
        }

        public ActionResult Remove(int productID)
        {
            return View();
        }
    }
}