using System.Web.Mvc;

namespace Zeus.Templates.Mvc.Controllers
{
	public abstract class WidgetController<T> : ZeusController<T>
		where T : ContentItem
	{
		public virtual ActionResult Header()
		{
			return new EmptyResult();
		}
	}
}