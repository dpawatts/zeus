using Zeus.AddIns.ECommerce.ContentTypes;
using Zeus.Templates.Mvc.Controllers;
using System.Web.Mvc;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
    public class ShopController : ZeusController<Shop>
    {
        public override ActionResult Index()
        {
            return base.Index();
        }

        public ActionResult ShoppingCart()
        {
            
        }
    }
}