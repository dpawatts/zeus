using Zeus.Web.Mvc.ViewModels;
using Zeus.Examples.MinimalMvcExample.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ViewModels
{
    public class CustomUrlContainerViewModel : ViewModel<CustomUrlContainer>
    {
        public CustomUrlContainerViewModel(CustomUrlContainer currentItem)
            : base(currentItem)
        {

        }
    }
}
