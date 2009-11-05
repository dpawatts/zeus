using System.Web.Mvc;
using Zeus.Integrity;
using Zeus.Templates.Mvc.ContentTypes.Forms;

namespace Zeus.Templates.ContentTypes.Forms
{
	[RestrictParents(typeof(Form))]
	[AllowedZones("Questions", "")]
	public abstract class Question : BaseWidget, IQuestion
	{
		public abstract TagBuilder CreateHtmlElement();

		public abstract string ElementID { get; }
		public abstract string GetAnswerText(string value);

		public virtual string QuestionText
		{
			get { return Title; }
		}
	}
}