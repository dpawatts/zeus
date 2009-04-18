using System;
using Zeus.FileSystem;

namespace Zeus.Admin.FileManager
{
	public partial class Upload : System.Web.UI.Page
	{
		protected void btnUploadFile_Click(object sender, EventArgs e)
		{
			if (uplFile.HasFile)
			{
				// If file with this name already exists, overwrite existing file.
				Folder folder = (Folder) Zeus.Context.Current.Resolve<Navigator>().Navigate(Request.QueryString["ParentPath"]);
				File file = folder.GetChild(uplFile.FileName) as File ?? new File();
				file.ContentType = uplFile.PostedFile.ContentType;
				file.Data = uplFile.FileBytes;
				file.Name = uplFile.FileName;

				file.AddTo(folder);

				Zeus.Context.Persister.Save(file);

				this.Page.ClientScript.RegisterStartupScript(typeof(Upload), "CloseThickBox", "self.parent.selectFile('" + Zeus.Web.Url.ToAbsolute(file.Url) + "');", true);
			}
		}
	}
}
