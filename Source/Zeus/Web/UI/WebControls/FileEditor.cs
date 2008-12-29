using System;
using System.Web.UI.WebControls;
using Zeus.FileSystem;
using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class FileEditor : FileUpload
	{
		private CheckBox chkClearFile;

		#region Properties

		public int ContentID
		{
			get { return (int) (this.ViewState["ContentID"] ?? 0); }
			set { this.ViewState["ContentID"] = value; }
		}

		public bool ShouldClear
		{
			get { return (chkClearFile != null && chkClearFile.Checked); }
		}


		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (this.Page.IsPostBack)
				EnsureChildControls();
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			this.Controls.Add(new LiteralControl("<br />"));

			if (this.ContentID != 0)
			{
				File file = Zeus.Context.Persister.Get<File>(this.ContentID);

				Span span = new Span { CssClass = "fileName" };
				span.Controls.Add(new Image { ImageUrl = file.IconUrl });
				span.Controls.Add(new LiteralControl(" " + file.Name));
				this.Controls.Add(span);

				this.Controls.Add(new LiteralControl("<br />"));

				chkClearFile = new CheckBox { ID = "chkClearFile", Text = "Clear", CssClass = "clearFile" };
				this.Controls.Add(chkClearFile);
			}
		}
	}
}
