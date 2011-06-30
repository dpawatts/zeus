using Zeus.Web.Mvc.ViewModels;
using Zeus.Examples.MinimalMvcExample.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ViewModels
{
	public class MyPageViewModel : ViewModel<MyPage>
	{
		public MyPageViewModel(MyPage currentItem, string param)
			: base(currentItem)
		{
            ParamPassed = param;
		}

        public string ParamPassed { get; set; }
	}
}
