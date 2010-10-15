using Zeus.Web.Mvc.ViewModels;
using Zeus.Examples.MinimalMvcExample.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ViewModels
{
    public class CustomUrlPageViewModel : ViewModel<CustomUrlPage>
    {
        public CustomUrlPageViewModel(CustomUrlPage currentItem)
            : base(currentItem)
        {

        }
    }
}
