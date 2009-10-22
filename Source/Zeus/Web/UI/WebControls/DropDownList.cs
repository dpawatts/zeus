using System;

namespace Zeus.Web.UI.WebControls
{
	public class DropDownList : System.Web.UI.WebControls.DropDownList
	{
		public new bool RequiresDataBinding
		{
			get { return (bool) (ViewState["RequiresDataBinding"] ?? false); }
			set { ViewState["RequiresDataBinding"] = value; }
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (this.RequiresDataBinding && !this.Page.IsPostBack)
				this.DataBind();

			base.OnPreRender(e);
		}
	}
}