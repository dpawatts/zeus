using System;
using Zeus.Templates.ContentTypes.Forms;
using System.Web.Mvc;
using Zeus.Templates.Services;
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

		public ActionResult Submit(FormCollection formValues)
		{
			// Loop through fields and extract values.
			string results = string.Empty;
			foreach (Question question in CurrentItem.Form.FormFields)
			{
				string answer = formValues[question.Name];
				results += question.Title + " = " + answer + Environment.NewLine;
			}

			// Send email.
			IEmailSender emailSender = Context.Current.Resolve<IEmailSender>();
			emailSender.SendEmail(TypedViewData.CurrentPage.Form.MailFrom,
				TypedViewData.CurrentPage.Form.MailTo,
				TypedViewData.CurrentPage.Form.MailSubject,
				results);
			
			// Show confirmation page.
			return View("Submit", TypedViewData);
		}
	}

	public interface IFormPageViewData : IViewData<FormPage>
	{

	}
}