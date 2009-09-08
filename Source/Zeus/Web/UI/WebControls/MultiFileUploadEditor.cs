using System;
using System.Web.UI;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.FileSystem;

namespace Zeus.Web.UI.WebControls
{
	public class MultiFileUploadEditor : BaseDetailCollectionEditor
	{
		#region Properties

		protected override string Title
		{
			get { return "File"; }
		}

		#endregion

		protected override Control CreateDetailEditor(int id, PropertyData detail)
		{
			LinkProperty linkDetail = detail as LinkProperty;

			FancyFileUpload fileUpload = CreateEditor();
			fileUpload.ID = ID + "_upl_" + id;

			if (linkDetail != null)
				fileUpload.CurrentFileName = ((File) linkDetail.LinkedItem).FileName;

			return fileUpload;
		}

		protected virtual FancyFileUpload CreateEditor()
		{
			return new FancyFileUpload();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
			writer.Write("<br style=\"clear:both\" />");
		}
	}
}