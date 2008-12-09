using System;
using Zeus.ContentTypes.Properties;

namespace Zeus.Tests.Definitions.Items
{
	[ContentType("Text page")]
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
