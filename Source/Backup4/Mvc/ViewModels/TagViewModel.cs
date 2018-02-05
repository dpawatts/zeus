using System.Collections.Generic;
using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class TagViewModel : ViewModel<Tag>
	{
		public TagViewModel(Tag currentItem, int referenceCount, IEnumerable<ContentItem> taggedItems)
			: base(currentItem)
		{
			ReferenceCount = referenceCount;
			TaggedItems = taggedItems;
		}

		public int ReferenceCount { get; set; }
		public IEnumerable<ContentItem> TaggedItems { get; set; }
	}
}