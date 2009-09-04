using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class ArchiveLinksViewModel : ViewModel<ArchiveLinks>
	{
		public ArchiveLinksViewModel(ArchiveLinks currentItem, IDictionary<BlogMonth, IEnumerable<Post>> archiveMonths)
			: base(currentItem)
		{
			ArchiveMonths = archiveMonths;
		}

		public IDictionary<BlogMonth, IEnumerable<Post>> ArchiveMonths { get; set; }
	}
}