using System.Web.Mvc;
using Zeus.Web.Mvc.Html;

namespace Zeus.Web.Mvc.ActionFilters
{
	public class ValidationGroupAttribute : ActionFilterAttribute
	{
		public string Name { get; set; }

		public ValidationGroupAttribute(string name)
		{
			Name = name;
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (!filterContext.Controller.ViewData.ModelState.IsValid)
				filterContext.Controller.ViewData[ValidationExtensions.FormNameKey] = Name;

			base.OnActionExecuted(filterContext);
		}
	}
}