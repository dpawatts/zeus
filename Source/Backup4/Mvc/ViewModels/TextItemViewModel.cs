using Zeus.Templates.ContentTypes;
using Zeus.Templates.ContentTypes.Widgets;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class TextItemViewModel : ViewModel<TextItem>
	{
		public TextItemViewModel(TextItem currentItem)
			: base(currentItem)
		{
			
		}
	}
}