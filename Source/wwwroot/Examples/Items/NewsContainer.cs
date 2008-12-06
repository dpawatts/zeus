using System;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Zeus.Examples.Items
{
	[ContentType("News Container", "NewsContainer", "A list of news. News items can be added to this page.", "", 150)]
	public class NewsContainer : ContentItem
	{
		[EditableFreeTextArea("Text", 100)]
		public virtual string Text
		{
			get { return GetDetail<string>("Text", string.Empty); }
			set { SetDetail("Text", value, string.Empty); }
		}

		public override string IconUrl
		{
			get { return "~/Examples/Assets/Images/Icons/newspaper_link.png"; }
		}

		public override string TemplateUrl
		{
			get { return "~/Examples/UI/Views/NewsContainer.aspx"; }
		}
	}
}
