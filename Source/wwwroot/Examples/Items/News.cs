using System;
using Zeus.Integrity;
using Zeus.Details;
using System.Web.UI.WebControls;

namespace Zeus.Templates.Items
{
	[Definition("News", "News", "A news page.", "", 155)]
	[RestrictParents(typeof(NewsContainer))]
	public class News : ContentItem
	{
		public News()
		{
			Visible = false;
		}

		[EditableTextBox("Introduction", 90, TextMode = TextBoxMode.MultiLine, Rows = 4, Columns = 80)]
		public virtual string Introduction
		{
			get { return (string) (GetDetail("Introduction") ?? string.Empty); }
			set { SetDetail("Introduction", value, string.Empty); }
		}

		public override string IconUrl
		{
			get { return "newspaper"; }
		}

		public override string TemplateUrl
		{
			get { return "NewsItem"; }
		}
	}
}
