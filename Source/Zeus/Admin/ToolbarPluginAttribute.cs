using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Admin
{
	public class ToolbarPluginAttribute : Attribute
	{
		public ToolbarPluginAttribute(string url, string target, string imageUrl, int sortOrder)
		{
			Url = url;
			Target = target;
			ImageUrl = imageUrl;
			SortOrder = sortOrder;
		}

		public string Url
		{
			get;
			set;
		}

		public string Target
		{
			get;
			set;
		}

		public string ImageUrl
		{
			get;
			set;
		}

		public int SortOrder
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}

		public string ToolTip
		{
			get;
			set;
		}

		public void AddTo(Control container)
		{
			HyperLink link = new HyperLink();
			link.NavigateUrl = this.Url;
			link.Target = this.Target;
			link.ToolTip = this.ToolTip;

			Image image = new Image();
			image.ImageUrl = this.ImageUrl;
			link.Controls.Add(image);

			if (!string.IsNullOrEmpty(Text))
				link.Controls.Add(new LiteralControl(this.Text));

			container.Controls.Add(link);
		}
	}
}
