using System.Reflection;
using System.Web.Mvc;

namespace Zeus.Web.Mvc
{
	public class AcceptParameterAttribute : ActionMethodSelectorAttribute
	{
		public string Name { get; set; }
		public string Value { get; set; }

		public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
		{
			var req = controllerContext.RequestContext.HttpContext.Request;
			return req.Form[Name] == Value;
		}
	}
}