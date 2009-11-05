using System.Web.Mvc;

namespace Zeus.Templates.ContentTypes.Forms
{
	[ContentType("Yes / No Question (checkbox)")]
	public class CheckBoxQuestion : Question
	{
		public override string ElementID
		{
			get { return "chk_" + ID; }
		}

		public override string GetAnswerText(string value)
		{
			return value;
		}

		public override TagBuilder CreateHtmlElement()
		{
			TagBuilder tagBuilder = new TagBuilder("input");
			tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.CheckBox), true);
			tagBuilder.MergeAttribute("id", ElementID, true);
			tagBuilder.MergeAttribute("name", ElementID, true);
			return tagBuilder;
		}
	}
}