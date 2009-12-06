using System.Linq;
using System.Web.Mvc;

namespace Zeus.Templates.ContentTypes.Forms
{
	[ContentType("Single Select (radio buttons)")]
	public class SingleSelectQuestion : OptionSelectQuestion
	{
		[ContentProperty("Display Vertically", 110)]
		public virtual bool Vertical
		{
			get { return GetDetail("Vertical", true); }
			set { SetDetail("Vertical", value); }
		}

		public override string ElementID
		{
			get { return "ss_" + ID; }
		}

		public override TagBuilder CreateHtmlElement()
		{
			var radioButtons = Options.Select(o =>
			{
				TagBuilder tagBuilder = new TagBuilder("input");
				tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Radio), true);
				tagBuilder.MergeAttribute("name", ElementID, true);
				tagBuilder.MergeAttribute("id", "ss_el_" + o.ID, true);
				tagBuilder.MergeAttribute("value", o.ID.ToString(), true);

				TagBuilder labelBuilder = new TagBuilder("label");
				labelBuilder.MergeAttribute("for", "ss_el_" + o.ID, true);
				labelBuilder.SetInnerText(o.Title);

				return tagBuilder.ToString() + labelBuilder.ToString();
			});

			TagBuilder result = new TagBuilder("span");
			result.MergeAttribute("class", "alternatives");
			result.SetInnerText(string.Join(string.Empty, radioButtons.ToArray()));
			return result;
		}

		public override string GetAnswerText(string value)
		{
			var selectedOption = Options.FirstOrDefault(opt => opt.ID.ToString() == value);

			if (selectedOption == null)
				return string.Empty;

			return selectedOption.Title;
		}
	}
}