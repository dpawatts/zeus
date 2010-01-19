using System.Web.Mvc;

namespace Zeus.Web.Mvc.ActionFilters
{
	public class ImportModelStateFromTempDataAttribute : ModelStateTempDataTransferAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			ModelStateDictionary modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;

			if (modelState != null)
			{
				// Only Import if we are viewing
				if (filterContext.Result is ViewResultBase)
				{
					filterContext.Controller.ViewData.ModelState.Merge(modelState);
				}
				else
				{
					//Otherwise remove it.
					filterContext.Controller.TempData.Remove(Key);
				}
			}

			base.OnActionExecuted(filterContext);
		}
	}
}