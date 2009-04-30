using Zeus.Design.Editors;

namespace Zeus.Tests.Definitions.Items
{
	[ContentType("Text page", "StartPage")]
	public class TestTextPage : ContentItem
	{
		[HtmlTextBoxEditor("Text", 100)]
		public virtual string Text
		{
			get { return (string) (GetDetail("Text") ?? string.Empty); }
			set { SetDetail("Text", value); }
		}
	}
}
