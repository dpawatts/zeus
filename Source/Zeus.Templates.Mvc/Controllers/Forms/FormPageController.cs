using System;
using Zeus.Templates.ContentTypes.Forms;
using System.Web.Mvc;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers.Forms
{
	[Controls(typeof(FormPage))]
	public class FormPageController : BaseController<FormPage, IFormPageViewData>
	{
		public override ActionResult Index()
		{
			return View("Index", TypedViewData);
		}

		public ActionResult Submit()
		{
			// Loop through fields and extract values.
			string results = string.Empty;
			foreach (Question question in CurrentItem.Form.FormFields)
			{
				string answer = HttpContext.Request.Form[question.Name];
				results += question.Title + " = " + answer + Environment.NewLine;
			}

			TempData["Results"] = results;

			return View("Submit", TypedViewData);
		}
	}

	public interface IFormPageViewData : IViewData<FormPage>
	{

	}
}