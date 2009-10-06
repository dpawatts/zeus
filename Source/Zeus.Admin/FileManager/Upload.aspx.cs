using System;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web;
using Zeus.FileSystem;

namespace Zeus.Admin.FileManager
{
	public partial class Upload : System.Web.UI.Page
	{
		protected void btnUploadFile_Click(object sender, EventArgs e)
		{
			if (uplFile.HasFile)
			{
				File file = new File();
				file.ContentType = uplFile.PostedFile.ContentType;
				file.Data = uplFile.FileBytes;
				file.Name = uplFile.FileName;

				Folder folder = (Folder) Zeus.Context.Current.Resolve<Navigator>().Navigate(Request.QueryString["ParentPath"]);
				file.AddTo(folder);

				Zeus.Context.Persister.Save(file);

				Page.ClientScript.RegisterStartupScript(typeof(Upload), "CloseThickBox", "self.parent.selectFile('" + Url.ToAbsolute(file.Url) + "');", true);
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(GetType(), "Zeus.Admin.Assets.Css.shared.css");
			base.OnPreRender(e);
		}
	}
}
