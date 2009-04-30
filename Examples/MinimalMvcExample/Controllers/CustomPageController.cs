using Zeus.Examples.MinimalMvcExample.ContentTypes;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.Examples.MinimalMvcExample.Controllers
{
	[Controls(typeof(CustomPage))]
	public class CustomPageController : BaseController<CustomPage, ICustomPageViewData>
	{
		
	}

	public interface ICustomPageViewData : IViewData<CustomPage>
	{
		
	}
}