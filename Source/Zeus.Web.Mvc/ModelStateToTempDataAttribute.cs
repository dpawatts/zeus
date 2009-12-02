using System.Web.Mvc;

namespace Zeus.Web.Mvc
{
	/// <summary>
	/// When a RedirectToRouteResult is returned from an action, anything in the ViewData.ModelState dictionary will be copied into TempData.
	/// When a ViewResult is returned from an action, any ModelState entries that were previously copied to TempData will be copied back to the ModelState dictionary.
	/// </summary>
	public class ModelStateToTempDataAttribute : ActionFilterAttribute
	{
		public const string TempDataKey = "__Zeus_ValidationFailures__";

		/// <summary>
		/// When a RedirectToRouteResult is returned from an action, anything in the ViewData.ModelState dictionary will be copied into TempData.
		/// When a ViewResult is returned from an action, any ModelState entries that were previously copied to TempData will be copied back to the ModelState dictionary.
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var modelState = filterContext.Controller.ViewData.ModelState;

			var controller = filterContext.Controller;

			if (filterContext.Result is ViewResultBase)
			{
				//If there are failures in tempdata, copy them to the modelstate
				CopyTempDataToModelState(controller.ViewData.ModelState, controller.TempData);
				return;
			}

			//If we're redirecting and there are errors, put them in tempdata instead (so they can later be copied back to modelstate)
			if ((filterContext.Result is RedirectToRouteResult || filterContext.Result is RedirectResult) && !modelState.IsValid)
			{
				CopyModelStateToTempData(controller.ViewData.ModelState, controller.TempData);
			}
		}

		private void CopyTempDataToModelState(ModelStateDictionary modelState, TempDataDictionary tempData)
		{
			if (!tempData.ContainsKey(TempDataKey)) return;

			var fromTempData = tempData[TempDataKey] as ModelStateDictionary;
			if (fromTempData == null) return;

			foreach (var pair in fromTempData)
			{
				if (modelState.ContainsKey(pair.Key))
				{
					modelState[pair.Key].Value = pair.Value.Value;

					foreach (var error in pair.Value.Errors)
					{
						modelState[pair.Key].Errors.Add(error);
					}
				}
				else
				{
					modelState.Add(pair.Key, pair.Value);
				}
			}
		}

		private static void CopyModelStateToTempData(ModelStateDictionary modelState, TempDataDictionary tempData)
		{
			tempData[TempDataKey] = modelState;
		}
	}
}