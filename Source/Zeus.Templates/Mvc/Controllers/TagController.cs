using System.Web.Mvc;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Templates.Services;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(Tag), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class TagController : ZeusController<Tag>
	{
		private readonly ITagService _tagService;

		public TagController(ITagService tagService)
		{
			_tagService = tagService;
		}

		public override ActionResult Index()
		{
			return View(new TagViewModel(CurrentItem,
				_tagService.GetReferenceCount(CurrentItem),
				_tagService.GetTaggedItems(CurrentItem)));
		}
	}
}