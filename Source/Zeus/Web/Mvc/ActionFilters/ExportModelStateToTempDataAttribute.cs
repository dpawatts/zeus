using System.Web.Mvc;

namespace Zeus.Web.Mvc.ActionFilters
{
	public class ExportModelStateToTempDataAttribute : ModelStateTempDataTransferAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			// Only export when ModelState is not valid
			if (!filterContext.Controller.ViewData.ModelState.IsValid)
			{
				//Export if we are redirecting
				if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
					filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
			}

			base.OnActionExecuted(filterContext);
		}
	}
}