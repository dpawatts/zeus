using System;

namespace Bermedia.Gibbons.Web.UI.UserControls
{
	public partial class PageNavigation : System.Web.UI.UserControl
	{
		protected Items.StartPage StartPage
		{
			get { return (Items.StartPage) Zeus.Find.StartPage; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			lsvSubNavigation.DataSource = this.StartPage.GetChildren<Web.Items.Page>();
			lsvSubNavigation.DataBind();
		}
	}
}