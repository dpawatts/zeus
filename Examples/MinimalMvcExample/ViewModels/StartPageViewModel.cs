using Zeus.Web.Mvc.ViewModels;
using Zeus.Examples.MinimalMvcExample.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ViewModels
{
    public class StartPageViewModel : ViewModel<StartPage>
    {
        public StartPageViewModel(StartPage currentItem)
            : base(currentItem)
        {

        }
    }
}
