using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Zeus.BaseLibrary.Web;
using Zeus.Net.Mail;
using System.Web.Mvc;
using Zeus.Templates.ContentTypes.Forms;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(Form), AreaName = TemplatesAreaRegistration.AREA_NAME)]
	public class FormController : ZeusController<Form>
	{
		private readonly IMailSender _mailSender;

		public FormController(IMailSender mailSender)
		{
			_mailSender = mailSender;
		}

		public override ActionResult Index()
		{
			if (Request.QueryString["sent"] == "true")
				return PartialView("Submit", new FormSubmitViewModel(CurrentItem));

			var elements = GetQuestions();
			return PartialView("Index", new FormViewModel(CurrentItem, elements));
		}

		private IEnumerable<IQuestion> GetQuestions()
		{
			return CurrentItem.GetChildren().OfType<IQuestion>();
		}

		public ActionResult Submit(FormCollection collection)
		{
			var sb = new StringBuilder(CurrentItem.MailBody + Environment.NewLine + Environment.NewLine);
			foreach (IQuestion q in GetQuestions())
			{
				sb.AppendFormat("{0}: {1}{2}", q.QuestionText, q.GetAnswerText(collection[q.ElementID]), Environment.NewLine);
			}
			var mm = new MailMessage(CurrentItem.MailFrom, CurrentItem.MailTo)
			{
				Subject = CurrentItem.MailSubject,
				Body = sb.ToString()
			};

			_mailSender.Send(mm);

			return Redirect(new Url(CurrentItem.Parent.Url).Query("sent", true));
		}
	}
}