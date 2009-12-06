using System.Linq;
using System.Web.Mvc;

namespace Zeus.Templates.ContentTypes.Forms
{
	[ContentType("Multiple Select (checkboxes)")]
	public class MultipleSelectQuestion : OptionSelectQuestion
	{
		[ContentProperty("Display Vertically", 110)]
		public virtual bool Vertical
		{
			get { return GetDetail("Vertical", true); }
			set { SetDetail("Vertical", value); }
		}

		public override string ElementID
		{
			get { return "ms_" + ID; }
		}

		public override TagBuilder CreateHtmlElement()
		{
			var checkboxes = Options.Select(o =>
			{
				TagBuilder tagBuilder = new TagBuilder("input");
				tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.CheckBox), true);
				tagBuilder.MergeAttribute("name", ElementID, true);
				tagBuilder.MergeAttribute("id", "ms_el_" + o.ID, true);
				tagBuilder.MergeAttribute("value", o.ID.ToString(), true);

				TagBuilder labelBuilder = new TagBuilder("label");
				labelBuilder.MergeAttribute("for", "ms_el_" + o.ID, true);
				labelBuilder.SetInnerText(o.Title);

				return tagBuilder.ToString() + labelBuilder.ToString();
			});

			TagBuilder result = new TagBuilder("span");
			result.MergeAttribute("class", "alternatives");
			result.SetInnerText(string.Join(string.Empty, checkboxes.ToArray()));
			return result;
		}

		public override string GetAnswerText(string value)
		{
			var values = (value ?? string.Empty).Split(',');

			var selectedOptions = Options.Where(opt => values.Contains(opt.ID.ToString())).Select(opt => opt.Title).ToArray();

			if (selectedOptions.Length == 0)
				return string.Empty;

			return string.Join(", ", selectedOptions);
		}
	}
}