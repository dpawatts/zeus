using System.Collections.Generic;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.ContentTypes.Widgets;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class TagCloudViewModel : ViewModel<TagCloud>
	{
		public TagCloudViewModel(TagCloud currentItem, IEnumerable<TagCloudEntry> entries)
			: base(currentItem)
		{
			Entries = entries;
		}

		public IEnumerable<TagCloudEntry> Entries { get; set; }
	}

	public class TagCloudEntry
	{
		public TagCloudEntry(Tag tag, int fontSize)
		{
			Tag = tag;
			FontSize = fontSize;
		}

		public Tag Tag { get; set; }
		public int FontSize { get; set; }
	}
}