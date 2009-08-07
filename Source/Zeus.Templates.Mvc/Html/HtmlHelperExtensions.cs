using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Microsoft.Web.Mvc;
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
					result += html.RadioButton(question.Name, option.Name, new { id = option.Name }) + " "
						+ html.Label(option.Name, option.Title);
					if (question.Vertical)
						result += "<br />";
					result += Environment.NewLine;
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

		public static string Label(this HtmlHelper html, string @for, string text)
		{
			return string.Format(@"<label for=""{0}"">{1}</label>", @for, text);
		}

		public static string Form(this HtmlHelper html, Form form)
		{
			StringWriter stringWriter = new StringWriter();

			stringWriter.WriteLine(form.IntroText);

			stringWriter.WriteLine(string.Format(@"<form action=""{0}"" method=""post"">", html.ViewContext.HttpContext.Request.RawUrl + "/submit"));
			foreach (Question question in form.FormFields)
			{
				stringWriter.WriteLine(html.Label(question.Name, question.Title));
				stringWriter.WriteLine(_fieldRenderers[question.GetType()].Invoke(html, question));
				stringWriter.WriteLine("<br />");
			}

			stringWriter.WriteLine(html.SubmitButton());
			stringWriter.WriteLine(@"</form>");

			return stringWriter.ToString();
		}

		public static string PropertyOrDefault(this HtmlHelper html, ContentItem contentItem, string propertyName, string fallbackPropertyName)
		{
			if (contentItem == null)
				return string.Empty;

			if (!contentItem.Details.ContainsKey(propertyName))
				return contentItem[fallbackPropertyName].ToString();

			return contentItem[propertyName].ToString();
		}

		public static string HtmlTitle(this HtmlHelper html, ContentItem contentItem)
		{
			return html.PropertyOrDefault(contentItem, SeoUtility.HTML_TITLE, "Title");
		}

		public static string PageTitle(this HtmlHelper html, ContentItem contentItem)
		{
			return html.PropertyOrDefault(contentItem, SeoUtility.PAGE_TITLE, "Title");
		}

		public static string MetaKeywords(this HtmlHelper html, ContentItem contentItem)
		{
			return html.PropertyOrDefault(contentItem, SeoUtility.META_KEYWORDS, "Title");
		}

		public static string MetaDescription(this HtmlHelper html, ContentItem contentItem)
		{
			return html.PropertyOrDefault(contentItem, SeoUtility.META_DESCRIPTION, "Title");
		}
	}
}