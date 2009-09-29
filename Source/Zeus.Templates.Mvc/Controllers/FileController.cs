using System.Web.Mvc;
using Zeus.FileSystem;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(File), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class FileController : ZeusController<File>
	{
		public override ActionResult Index()
		{
			return new FileContentResult(CurrentItem.Data, CurrentItem.ContentType);
		}
	}
}