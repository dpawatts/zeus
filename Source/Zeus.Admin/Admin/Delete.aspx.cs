using System;
using Isis.Web;

namespace Zeus.Admin
{
	public partial class Delete : AdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			int id = Request.GetRequiredInt("id");
			ContentItem contentItem = Zeus.Context.Persister.Get(id);
			ContentItem parent = contentItem.Parent;
			Zeus.Context.Persister.Delete(contentItem);
			Refresh(parent, false);
		}
	}
}
