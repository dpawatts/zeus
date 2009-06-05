using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Zeus.Templates.ContentTypes.Forms;
using System.IO;

namespace Zeus.Templates.Mvc.Html
{
	public static class HtmlHelperExtensions
	{
		private static Dictionary<Type, Func<HtmlHelper, Question, string>> _fieldRenderers;

		static HtmlHelperExtensions()
		{
			_fieldRenderers = new Dictionary<Type, Func<HtmlHelper, Question, string>>();

			_fieldRenderers[typeof(TextQuestion)] = (html, q) =>
				{
					TextQuestion question = (TextQuestion) q;
					if (question.Rows > 1)
						return html.TextArea(question.Name, null,
							question.Rows,
							question.Columns.Value, null);
					return html.TextBox(question.Name);
				};

			_fieldRenderers[typeof(CheckBoxQuestion)] = (html, q) =>
				{
					CheckBoxQuestion question = (CheckBoxQuestion) q;
					return html.CheckBox(question.Name);
				};

			_fieldRenderers[typeof(SingleSelectQuestion)] = (html, q) =>
			{
				SingleSelectQuestion question = (SingleSelectQuestion)q;
				string result = string.Empty;
				foreach (Option option in question.Options)
				{
					result += html.RadioButton(question.Name, option.Title) + " ";
					if (question.Vertical)
						result += "<br />";
				}
				return result;
			};

			_fieldRenderers[typeof(MultipleSelectQuestion)] = (html, q) =>
			{
				MultipleSelectQuestion question = (MultipleSelectQuestion)q;
				string result = string.Empty;
				foreach (Option option in question.Options)
				{
					result += html.CheckBox(option.Name) + " ";
					if (question.Vertical)
						result += "<br />";
				}
				return result;
			};
		}

		public static string RenderDynamicForm(this HtmlHelper html, Form form)
		{
			StringWriter stringWriter = new StringWriter();

			stringWriter.WriteLine(form.IntroText);

			foreach (Question question in form.FormFields)
			{
				stringWriter.WriteLine(string.Format(@"<label for=""{0}"">{1}</label>", question.Name, question.Title));
				stringWriter.WriteLine(_fieldRenderers[question.GetType()].Invoke(html, question));
			}

			return stringWriter.ToString();
		}
	}
}