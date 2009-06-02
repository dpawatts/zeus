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
            _fieldRenderers[typeof (TextQuestion)] = (html, q) =>
                                                         {
                                                             TextQuestion textQuestion = (TextQuestion) q;
                                                             if (textQuestion.Rows > 1)
                                                                 return html.TextArea(textQuestion.Name, null,
                                                                                      textQuestion.Rows,
                                                                                      textQuestion.Columns.Value, null);
                                                             return html.TextBox(textQuestion.Name);
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