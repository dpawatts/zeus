using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Admin
{
	public class ToolbarPluginAttribute : Attribute
	{
		public ToolbarPluginAttribute(string url, string target, string imageUrl, int sortOrder)
		{
			this.Url = url;
			this.Target = target;
			this.ImageUrl = imageUrl;
			this.SortOrder = sortOrder;
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

			container.Controls.Add(link);
		}
	}
}
