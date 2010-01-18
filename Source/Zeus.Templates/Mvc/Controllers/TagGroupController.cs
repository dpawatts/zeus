using System.Linq;
using System.Web.Mvc;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Templates.Services;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(TagGroup), AreaName = TemplatesAreaRegistration.AREA_NAME)]
	public class TagGroupController : ZeusController<TagGroup>
	{
		private readonly ITagService _tagService;

		public TagGroupController(ITagService tagService)
		{
			_tagService = tagService;
		}

		public override ActionResult Index()
		{
			var activeTags = _tagService.GetActiveTags(_tagService.GetCurrentTagGroup(CurrentItem))
				.Select(t => new ActiveTag(t, _tagService.GetReferenceCount(t)));
			return View(new TagGroupViewModel(CurrentItem, activeTags));
		}
	}
}