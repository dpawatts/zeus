using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.FileSystem;
using System.Web.UI.HtmlControls;

namespace Zeus.Admin.FileManager
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			IFileSystem fileSystem = Zeus.Context.Current.Resolve<IFileSystem>();
			HtmlGenericControl ul = new HtmlGenericControl("ul");
			ul.Attributes["class"] = "simpleTree";
			BuildTree(fileSystem.RootFolder, ul, true);
			plcFileTree.Controls.Add(ul);

			/*IFileSystem fileSystem = Zeus.Context.Current.Resolve<IFileSystem>();
			rptFiles.DataSource = fileSystem.RootFolder.Files;
			rptFiles.DataBind();*/
		}

		private void BuildTree(IFolder folder, Control parentControl, bool root)
		{
			HtmlGenericControl li = new HtmlGenericControl("li");
			if (root)
				li.Attributes["class"] = "root";

			HtmlImage image = new HtmlImage();
			image.Src = "~/admin/assets/images/icons/folder.png";

			LinkButton linkButton = new LinkButton();
			linkButton.Controls.Add(image);
			linkButton.Controls.Add(new LiteralControl(folder.Name));

			HtmlGenericControl span = new HtmlGenericControl("span");
			span.Controls.Add(linkButton);
			li.Controls.Add(span);

			if (folder.Folders.Count > 0)
			{
				HtmlGenericControl ul = new HtmlGenericControl("ul");
				foreach (IFolder childFolder in folder.Folders)
					BuildTree(childFolder, ul, false);
				li.Controls.Add(ul);
			}

			parentControl.Controls.Add(li);
		}

		/*protected void btnUploadFile_Click(object sender, EventArgs e)
		{
			if (uplFile.HasFile)
			{
				IFileSystem fileSystem = Zeus.Context.Current.Resolve<IFileSystem>();
				IFileIdentifier fileIdentifier = fileSystem.AddFile(uplFile.FileName, uplFile.FileBytes);

				rptFiles.DataBind();

				this.ClientScript.RegisterStartupScript(typeof(Default), "UploadFile", "file_onClick($('#file" + fileIdentifier + "'));", true);
			}
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(hdnSelectedFileID.Value))
			{
				IFileSystem fileSystem = Zeus.Context.Current.Resolve<IFileSystem>();
				IFileIdentifier fileIdentifier = fileSystem.ParseIdentifier(hdnSelectedFileID.Value);
				fileSystem.DeleteFile(fileIdentifier);

				rptFiles.DataBind();
			}
		}*/
	}
}
