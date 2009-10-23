using System.Collections.Generic;
using Zeus.Templates.Mvc.ContentTypes.Forms;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class FormViewModel : ViewModel<Form>
	{
		public FormViewModel(Form currentItem, IEnumerable<IQuestion> elements)
			: base(currentItem)
		{
			Elements = elements;
		}

		public IEnumerable<IQuestion> Elements { get; private set; }
	}
}