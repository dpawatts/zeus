using System;
using System.Web.UI.WebControls;
using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Sources;
using SoundInTheory.DynamicImage.Filters;
using System.Web.UI;
using Zeus.FileSystem.Images;
using Unit = SoundInTheory.DynamicImage.Unit;

namespace Zeus.Web.UI.WebControls
{
	public class ImageEditor : FileUpload
	{
		private CheckBox chkClearImage;

		#region Properties

		public int ContentID
		{
			get { return (int) (this.ViewState["ContentID"] ?? 0); }
			set { this.ViewState["ContentID"] = value; }
		}

		public bool ClearImage
		{
			get
			{
				return chkClearImage != null && chkClearImage.Checked;
			}
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
				// Add DynamicImage control to render existing image.
				this.Controls.Add(new DynamicImage
				                  	{
				                  		CssClass = "image",
				                  		Layers = new LayerCollection
				                  		         	{
				                  		         		new ImageLayer
				                  		         			{
				                  		         				Source = new ImageSourceCollection
				                  		         				         	{
				                  		         				         		new ZeusImageSource { ContentID = this.ContentID }
				                  		         				         	},
				                  		         				Filters = new FilterCollection
				                  		         				          	{
				                  		         				          		new ResizeFilter { Width = Unit.Pixel(200), Height = Unit.Pixel(200) }
				                  		         				          	}
				                  		         			}
				                  		         	}
				                  	});

				this.Controls.Add(new LiteralControl("<br />"));

				chkClearImage = new CheckBox { ID = ID + "chkClearImage", Text = "Clear", CssClass = "clearFile" };
				this.Controls.Add(chkClearImage);
			}
		}
	}
}