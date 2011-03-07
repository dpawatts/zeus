using System.Web.UI.WebControls;
using System.Web.UI;

namespace Zeus.BaseLibrary.Web.UI.HtmlControls
{
	[ConstructorNeedsTag(false)]
	public class SortableColumnHeader : System.Web.UI.HtmlControls.HtmlTableCell
	{
		public bool IncludedInSort
		{
			get
			{
				object value = this.ViewState["IncludedInSort"];
				if (value == null)
					return false;
				else
					return (bool) value;
			}
			set
			{
				this.ViewState["IncludedInSort"] = value;
				ChildControlsCreated = false;
			}
		}

		public SortDirection SortDirection
		{
			get
			{
				object value = this.ViewState["SortDirection"];
				if (value == null)
					return SortDirection.Ascending;
				else
					return (SortDirection) value;
			}
			set
			{
				this.ViewState["SortDirection"] = value;
			}
		}

		public SortableColumnHeader()
			: base("th")
		{

		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			if (this.IncludedInSort)
				this.Attributes["class"] = "on " + ((this.SortDirection == SortDirection.Ascending) ? "up" : "down");
			else
				this.Attributes["class"] = string.Empty;
		}
	}
}
