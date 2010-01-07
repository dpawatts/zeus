using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class ArchiveCalendarWidgetViewModel : ViewModel<ArchiveCalendarWidget>
	{
		public ArchiveCalendarWidgetViewModel(ArchiveCalendarWidget currentItem)
			: base(currentItem)
		{
			
		}
	}
}