using System.Web.Mvc;

namespace Zeus.Web.Mvc.ActionFilters
{
	public abstract class ModelStateTempDataTransferAttribute : ActionFilterAttribute
	{
		protected static readonly string Key = typeof(ModelStateTempDataTransferAttribute).FullName;
	}
}