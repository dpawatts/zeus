using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.Linq;
using System.Linq.Dynamic;

namespace Zeus.Admin
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
	public class GridViewPluginAttribute : Attribute
	{
		public GridViewPluginAttribute(Type type, string name, string url, string target, string text, int sortOrder)
		{
			this.Type = type;
			this.Name = name;
			this.Url = url;
			this.Target = target;
			this.Text = text;
			this.SortOrder = sortOrder;
		}

		public Type Type
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
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

		public string Text
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

		public string Expression
		{
			get;
			set;
		}

		public void AddTo(Control container)
		{
			HyperLink link = new HyperLink();
			link.Target = this.Target;
			link.ToolTip = this.ToolTip;
			link.Text = this.Text;

			container.Controls.Add(link);
		}

		public void SetValue(Control container, ContentItem selectedItem)
		{
			if (!string.IsNullOrEmpty(this.Expression))
			{
				Array sourceArray = Array.CreateInstance(selectedItem.GetType(), 1);
				sourceArray.SetValue(selectedItem, 0);
				IQueryable results = sourceArray.AsQueryable().Where(this.Expression);

				container.Visible = ((HyperLink) container.Controls[0]).Visible = results.Any();
			}
			((HyperLink) container.Controls[0]).NavigateUrl = this.Url.Replace("{selected}", HttpUtility.UrlEncode(selectedItem.Path));
		}
	}
}
