using System.IO;
using System.Reflection;
using System.Web.Mvc;

namespace Zeus.Web.Mvc.Pdf
{
	public static class ControllerExtensions
	{
		public static PdfViewResult PdfView(this Controller controller, object model)
		{
			return new PdfViewResult
			{
				ViewData = new ViewDataDictionary(model)
			};
		}

		public static PdfViewResult PdfView(this Controller controller, Stream template, Assembly[] resourceAssemblies, object model)
		{
			return new PdfViewResult(template, resourceAssemblies)
			{
				ViewData = new ViewDataDictionary(model)
			};
		}
	}
}