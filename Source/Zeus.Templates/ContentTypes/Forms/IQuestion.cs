using MvcContrib.UI;

namespace Zeus.Templates.Mvc.ContentTypes.Forms
{
	public interface IQuestion
	{
		IElement CreateHtmlElement();
		string QuestionText { get; }
		string ElementID { get; }
		string GetAnswerText(string value);
	}
}