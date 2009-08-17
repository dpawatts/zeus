using MvcContrib.UI;

namespace Zeus.Templates.Mvc.ContentTypes.Forms
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

		public override IElement CreateHtmlElement()
		{
			return new MvcContrib.UI.Tags.CheckBoxField
			{
				Id = ElementID,
				Name = ElementID
			};
		}
	}
}