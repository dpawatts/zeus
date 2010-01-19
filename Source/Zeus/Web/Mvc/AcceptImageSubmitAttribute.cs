using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Zeus.Web.Mvc
{
	public class AcceptImageSubmitAttribute : ActionMethodSelectorAttribute
	{
		public string Name { get; set; }

		public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
		{
			var button = controllerContext.HttpContext.Request.Form.Keys
				.OfType<string>()
				.Where(x => x.EndsWith(".x"))
				.SingleOrDefault();

			// we got something like addToCart.x
			if (button == null)
				throw new ZeusException("No image button found");

			string buttonName = button.Substring(0, button.Length - 2);
			return (Name == buttonName);
		}
	}
}