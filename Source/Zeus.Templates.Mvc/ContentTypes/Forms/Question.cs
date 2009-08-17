using MvcContrib.UI;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.Templates.Mvc.ContentTypes.Forms
{
	[RestrictParents(typeof(Form))]
	[AllowedZones("Questions", "")]
	public abstract class Question : BaseWidget, IQuestion
	{
		public abstract IElement CreateHtmlElement();

		public abstract string ElementID { get; }
		public abstract string GetAnswerText(string value);

		public virtual string QuestionText
		{
			get { return Title; }
		}
	}
}