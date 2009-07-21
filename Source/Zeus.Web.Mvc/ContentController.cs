﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Castle.Components.DictionaryAdapter;
using Zeus.Engine;
using Zeus.Security;

namespace Zeus.Web.Mvc
{
	/// <summary>
	/// Base class for content controllers that provides easy access to the content item in scope.
	/// </summary>
	/// <typeparam name="T">The type of content item the controller handles.</typeparam>
	public abstract class ContentController<T> : Controller
		where T : ContentItem
	{
		private T _currentItem;

		protected ContentController()
		{
			TempDataProvider = new SessionAndPerRequestTempDataProvider();
		}

		protected virtual ContentEngine Engine
		{
			get
			{
				return ControllerContext.RouteData.Values[ContentRoute.ContentEngineKey] as ContentEngine
					?? Context.Current;
			}
		}

		/// <summary>The content item associated with the requested path.</summary>
		public virtual T CurrentItem
		{
			get
			{
				if (_currentItem == null)
				{
					_currentItem = ControllerContext.RouteData.Values[ContentRoute.ContentItemKey] as T
												 ?? GetCurrentItemById();
				}
				return _currentItem;
			}
			set { _currentItem = value; }
		}

		protected ContentItem CurrentPage
		{
			get
			{
				ContentItem page = CurrentItem;
				while (page != null && !page.IsPage)
					page = page.Parent;

				return page;
			}
		}

		private T GetCurrentItemById()
		{
			int itemId;
			if (Int32.TryParse(ControllerContext.RouteData.Values[ContentRoute.ContentItemIdKey] as string, out itemId))
				return Engine.Persister.Get(itemId) as T;

			return null;
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (CurrentItem != null)
			{
				var securityManager = Engine.Resolve<ISecurityManager>();

				if (!securityManager.IsAuthorized(CurrentItem, User, Operations.Read))
					filterContext.Result = new HttpUnauthorizedResult();
			}

			base.OnActionExecuting(filterContext);
		}

		/// <summary>Defaults to the current item's TemplateUrl and pass the item itself as view data.</summary>
		/// <returns>A reference to the item's template.</returns>
		public virtual ActionResult Index()
		{
			return View(CurrentItem);
		}

		protected virtual ActionResult RedirectToParentPage()
		{
			return Redirect(CurrentPage.Url);
		}

		/// <summary>
		/// Returns a <see cref="ViewPageResult"/> which calls the default action for the controller that handles the current page.
		/// </summary>
		/// <returns></returns>
		protected virtual ViewPageResult ViewParentPage()
		{
			if (CurrentItem.IsPage)
			{
				throw new InvalidOperationException(
					"The current page is already being rendered. ViewPage should only be used from content items to render their parent page.");
			}

			return new ViewPageResult(CurrentPage, Engine.Resolve<IControllerMapper>(), Engine.Resolve<IWebContext>(),
																ActionInvoker);
		}

		#region Nested type: SessionAndPerRequestTempDataProvider

		/// <summary>
		/// Overrides the default behaviour in the SessionStateTempDataProvider to make TempData available for the entire request, not just for the first controller it sees
		/// </summary>
		private sealed class SessionAndPerRequestTempDataProvider : ITempDataProvider
		{
			private const string TempDataSessionStateKey = "__ControllerTempData";

			#region ITempDataProvider Members

			public IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
			{
				HttpContextBase httpContext = controllerContext.HttpContext;

				if (httpContext.Session == null)
				{
					throw new InvalidOperationException("Session state appears to be disabled.");
				}

				var tempDataDictionary = (httpContext.Session[TempDataSessionStateKey]
																	?? httpContext.Items[TempDataSessionStateKey])
																 as Dictionary<string, object>;

				if (tempDataDictionary == null)
				{
					return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				}

				// If we got it from Session, remove it so that no other request gets it
				httpContext.Session.Remove(TempDataSessionStateKey);
				// Cache the data in the HttpContext Items to keep available for the rest of the request
				httpContext.Items[TempDataSessionStateKey] = tempDataDictionary;

				return tempDataDictionary;
			}

			public void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
			{
				HttpContextBase httpContext = controllerContext.HttpContext;

				if (httpContext.Session == null)
				{
					throw new InvalidOperationException("Session state appears to be disabled.");
				}

				httpContext.Session[TempDataSessionStateKey] = values;
			}

			#endregion
		}

		#endregion
	}
}
