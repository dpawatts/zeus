using System.Web.Mvc;
using Castle.Components.DictionaryAdapter;
using Zeus.Engine;

namespace Zeus.Web.Mvc
{
	/// <summary>
	/// Base class for content controllers that provides easy access to the content item in scope.
	/// </summary>
	/// <typeparam name="T">The type of content item the controller handles.</typeparam>
	public abstract class ContentController<T, TViewData> : Controller
		where T : ContentItem
		where TViewData : class
	{
		private TViewData _typedViewData;

		protected ContentController()
		{
			// TODO - allow custom hooks to populate view data at this point.
		}

		protected virtual ContentEngine Engine
		{
			get { return ControllerContext.RouteData.Values[ContentRoute.ContentEngineKey] as ContentEngine; }
		}

		protected TViewData TypedViewData
		{
			get
			{
				if (_typedViewData == null)
					_typedViewData = new DictionaryAdapterFactory().GetAdapter<TViewData>(new ViewDataDictionaryWrapper(ViewData));
				return _typedViewData;
			}
		}

		protected TCustomViewData GetCustomTypedViewData<TCustomViewData>()
		{
			return new DictionaryAdapterFactory().GetAdapter<TCustomViewData>(new ViewDataDictionaryWrapper(ViewData));
		}

		/// <summary>The content item associated with the requested path.</summary>
		protected virtual T CurrentItem
		{
			get { return ControllerContext.RouteData.Values[ContentRoute.ContentItemKey] as T; }
		}

		/// <summary>Defaults to the current item's TemplateUrl and pass the item itself as view data.</summary>
		/// <returns>A reference to the item's template.</returns>
		public virtual ActionResult Index()
		{
			return View(CurrentItem.FindPath(PathData.DefaultAction).TemplateUrl, CurrentItem);
		}
	}
}
