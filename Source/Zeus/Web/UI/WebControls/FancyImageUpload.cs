using System;
using System.Drawing;
using System.Web.UI;
using Zeus.Design.Editors;

namespace Zeus.Web.UI.WebControls
{
	public class FancyImageUpload : FancyFileUpload, IValidator
	{
		public int? MinimumWidth
		{
			get { return ViewState["MinimumWidth"] as int?; }
			set { ViewState["MinimumWidth"] = value; }
		}

		public int? MinimumHeight
		{
			get { return ViewState["MinimumHeight"] as int?; }
			set { ViewState["MinimumHeight"] = value; }
		}

		protected override void OnInit(EventArgs e)
		{
			Page.Validators.Add(this);
			base.OnInit(e);
		}

		#region IValidator Members

		/// <summary>Gets or sets the error message generated when the name editor contains invalid values.</summary>
		public string ErrorMessage
		{
			get { return (string) ViewState["ErrorMessage"] ?? string.Empty; }
			set { ViewState["ErrorMessage"] = value; }
		}

		/// <summary>Gets or sets whether the name editor's value passes validaton.</summary>
		public bool IsValid
		{
			get { return ViewState["IsValid"] != null ? (bool) ViewState["IsValid"] : true; }
			set { ViewState["IsValid"] = value; }
		}

		/// <summary>Validates the name editor's value checking uniqueness and lenght.</summary>
		public void Validate()
		{
			if (!HasNewOrChangedFile)
				return;

			if (MinimumWidth == null && MinimumHeight == null)
				return;

			using (Bitmap bitmap = new Bitmap(FileUploadEditorAttribute.GetUploadedFilePath(this)))
			{
				if (MinimumWidth != null && bitmap.Width < MinimumWidth)
				{
					ErrorMessage = "Image width is less than the minimum width of " + MinimumWidth + " pixels.";
					IsValid = false;
					return;
				}

				if (MinimumHeight != null && bitmap.Height < MinimumHeight)
				{
					ErrorMessage = "Image height is less than the minimum height of " + MinimumHeight + " pixels.";
					IsValid = false;
					return;
				}
			}

			IsValid = true;
		}

		#endregion
	}
}