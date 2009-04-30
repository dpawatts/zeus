using System.Web.Mvc;
using Castle.Components.DictionaryAdapter;

namespace Zeus.Web.Mvc
{
	public class ContentViewPage<TViewData> : ViewPage
		where TViewData : class
	{
		private TViewData _typedViewData;

		protected TViewData TypedViewData
		{
			get
			{
				if (_typedViewData == null)
					_typedViewData = new DictionaryAdapterFactory().GetAdapter<TViewData>(new ViewDataDictionaryWrapper(base.ViewData));
				return _typedViewData;
			}
		}
	}
}