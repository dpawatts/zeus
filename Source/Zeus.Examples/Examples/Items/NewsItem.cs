using System;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using System.Web.UI.WebControls;

namespace Zeus.Examples.Items
{
	[ContentType("News", "News", "A news page.", "", 155)]
	[RestrictParents(typeof(NewsContainer))]
	public class NewsItem : ContentItem
	{
		public NewsItem()
		{
			Visible = false;
		}

		[TextBoxEditor("Introduction", 90, TextMode = TextBoxMode.MultiLine, Rows = 4, Columns = 80)]
		[LiteralDisplayer]
		public virtual string Introduction
		{
			get { return (string) (GetDetail("Introduction") ?? string.Empty); }
			set { SetDetail("Introduction", value, string.Empty); }
		}

		public override string IconUrl
		{
			get { return "~/Examples/Assets/Images/Icons/newspaper.png"; }
		}

		public override string TemplateUrl
		{
			get { return "~/Examples/UI/Views/NewsItem.aspx"; }
		}
	}
}
