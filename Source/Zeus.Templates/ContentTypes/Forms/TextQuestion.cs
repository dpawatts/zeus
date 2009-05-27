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
	}
}