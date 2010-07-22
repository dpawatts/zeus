using Zeus.Web.Mvc.ViewModels;
using Zeus.Examples.MinimalMvcExample.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ViewModels
{
	public class MyPageViewModel : ViewModel<MyPage>
	{
		public MyPageViewModel(MyPage currentItem)
			: base(currentItem)
		{

		}
	}
}
