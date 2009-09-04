using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(ArchiveLinks), AreaName = BlogsWebPackage.AREA_NAME)]
	public class ArchiveLinksController : ZeusController<ArchiveLinks>
	{
		public override ActionResult Index()
		{
			var archiveMonths = Find.EnumerateAccessibleChildren(CurrentItem.Blog)
				//.Where(p => p.IsPublished())
				.OfType<BlogMonth>()
				.OrderByDescending(bm => bm.Date)
				.ToDictionary(bm => bm, bm => bm.GetChildren<Post>());
			return PartialView(new ArchiveLinksViewModel(CurrentItem, archiveMonths));
		}
	}
}