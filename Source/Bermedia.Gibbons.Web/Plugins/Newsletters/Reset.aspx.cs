using System;
using System.Linq;
using Isis.Web;
using Isis.Data.Linq;
using Zeus.Admin;
using Bermedia.Gibbons.Web.Items;

namespace Bermedia.Gibbons.Web.Plugins.Newsletters
{
	public partial class Reset : AdminPage
	{
		protected Newsletter SelectedNewsletter
		{
			get { return this.SelectedItem as Newsletter; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			foreach (NewsletterLogEntry logEntry in this.SelectedNewsletter.LogEntries)
				Zeus.Context.Persister.Delete(logEntry);

			this.SelectedNewsletter.TotalMessages = 0;
			this.SelectedNewsletter.CurrentMessage = 0;
			this.SelectedNewsletter.Status = NewsletterStatus.NotStarted;
			this.SelectedNewsletter.ErrorMessage = null;

			Zeus.Context.Persister.Save(this.SelectedNewsletter);

			Response.Redirect("send.aspx?selected=" + Server.UrlEncode(Request.QueryString["selected"]));
		}
	}
}