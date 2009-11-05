using System.Web.Mvc;

namespace Zeus.Templates.ContentTypes.Forms
{
	[ContentType("Text Question (textbox)")]
	public class TextQuestion : Question
	{
		[ContentProperty("Rows", 110)]
		public virtual int Rows
		{
			get { return GetDetail("Rows", 1); }
			set { SetDetail("Rows", value); }
		}

		[ContentProperty("Columns", 120)]
		public virtual int? Columns
		{
			get { return GetDetail<int?>("Columns", null); }
			set { SetDetail("Columns", value); }
		}

		public override string ElementID
		{
			get { return "txt_" + ID; }
		}

		public override string GetAnswerText(string value)
		{
			return value;
		}

		public override TagBuilder CreateHtmlElement()
		{
			TagBuilder tagBuilder = new TagBuilder("textarea");
			tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Text), true);
			tagBuilder.MergeAttribute("id", ElementID, true);
			tagBuilder.MergeAttribute("name", ElementID, true);
			tagBuilder.MergeAttribute("rows", Rows.ToString(), true);
			if (Columns != null)
				tagBuilder.MergeAttribute("cols", Columns.Value.ToString(), true);
			return tagBuilder;
		}
	}
}