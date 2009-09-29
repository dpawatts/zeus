using System;
using System.Linq;
using System.Web.Mvc;
using Zeus.Templates.ContentTypes.Widgets;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Templates.Services;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(TagCloud), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class TagCloudController : WidgetController<TagCloud>
	{
		private readonly ITagService _tagService;

		public TagCloudController(ITagService tagService)
		{
			_tagService = tagService;
		}

		public override ActionResult Index()
		{
			// Get active tags with their reference counts.
			var activeTagsCounts = _tagService.GetActiveTags(_tagService.GetCurrentTagGroup(CurrentItem))
				.Select(t => new { Tag = t, ReferenceCount = _tagService.GetReferenceCount(t) });

			// Get the min and max reference counts.
			int minReferenceCount = activeTagsCounts.Min(atc => atc.ReferenceCount);
			int maxReferenceCount = activeTagsCounts.Max(atc => atc.ReferenceCount);
			double logMin = Math.Log(minReferenceCount);
			double logDiff = Math.Log(maxReferenceCount) - logMin;
			int diffFontSize = CurrentItem.MaxFontSize - CurrentItem.MinFontSize;

			//weight = (Math.log(occurencesOfCurrentTag)-Math.log(minOccurs))/(Math.log(maxOccurs)-Math.log(minOccurs));
//fontSizeOfCurrentTag = minFontSize + Math.round((maxFontSize-minFontSize)*weight)

			var tagCloudEntries = activeTagsCounts.Select(atc =>
			{
				double weight = (Math.Log(atc.ReferenceCount) - logMin)/logDiff;
				int fontSize = CurrentItem.MinFontSize + (int) Math.Round(diffFontSize*weight);
				return new TagCloudEntry(atc.Tag, fontSize);
			});

			return PartialView(new TagCloudViewModel(CurrentItem, tagCloudEntries));
		}
	}
}