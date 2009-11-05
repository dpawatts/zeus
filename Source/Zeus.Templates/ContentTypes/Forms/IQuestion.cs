using System.Web.Mvc;

namespace Zeus.Templates.ContentTypes.Forms
{
	public interface IQuestion
	{
		TagBuilder CreateHtmlElement();
		string QuestionText { get; }
		string ElementID { get; }
		string GetAnswerText(string value);
	}
}