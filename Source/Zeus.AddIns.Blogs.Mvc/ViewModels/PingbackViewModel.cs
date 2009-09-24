using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class PingbackViewModel : ViewModel<Pingback>
	{
		public PingbackViewModel(Pingback currentItem)
			: base(currentItem)
		{
			
		}
	}
}