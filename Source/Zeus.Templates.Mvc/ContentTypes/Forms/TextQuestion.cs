using MvcContrib.UI;

namespace Zeus.Templates.Mvc.ContentTypes.Forms
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

		public override IElement CreateHtmlElement()
		{
			var textArea = new MvcContrib.UI.Tags.TextArea
			{
				Id = ElementID,
				Name = ElementID,
				Rows = Rows
			};
			if (Columns != null)
				textArea.Cols = Columns.Value;
			return textArea;
		}
	}
}