using System.Collections.Generic;
using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class TagGroupViewModel : ViewModel<TagGroup>
	{
		public TagGroupViewModel(TagGroup currentItem, IEnumerable<ActiveTag> activeTags)
			: base(currentItem)
		{
			ActiveTags = activeTags;
		}

		public IEnumerable<ActiveTag> ActiveTags { get; set; }
	}

	public class ActiveTag
	{
		public ActiveTag(Tag tag, int referenceCount)
		{
			Tag = tag;
			ReferenceCount = referenceCount;
		}

		public Tag Tag { get; set; }
		public int ReferenceCount { get; set; }
	}
}