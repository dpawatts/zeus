using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Zeus.Net.Mail;
using System.Web.Mvc;
using Zeus.Templates.Mvc.ContentTypes.Forms;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers.Forms
{
	[Controls(typeof(FormPage), AreaName = "Templates")]
	public class FormPageController : ZeusController<FormPage>
	{
		private readonly IMailSender _mailSender;

		public FormPageController(IMailSender mailSender)
		{
			_mailSender = mailSender;
		}

		public override ActionResult Index()
		{
			var elements = GetQuestions();
			return View(new FormViewModel(CurrentItem.Form, elements));
		}

		private IEnumerable<IQuestion> GetQuestions()
		{
			return CurrentItem.Form.GetChildren().OfType<IQuestion>();
		}

		public ActionResult Submit(FormCollection collection)
		{
			var sb = new StringBuilder(CurrentItem.Form.MailBody);
			foreach (IQuestion q in GetQuestions())
			{
				sb.AppendFormat("{0}: {1}{2}", q.QuestionText, q.GetAnswerText(collection[q.ElementID]), Environment.NewLine);
			}
			var mm = new MailMessage(CurrentItem.Form.MailFrom, CurrentItem.Form.MailTo)
			{
				Subject = CurrentItem.Form.MailSubject,
				Body = sb.ToString()
			};

			_mailSender.Send(mm);

			return View(new FormViewModel(CurrentItem.Form, null));
		}
	}
}