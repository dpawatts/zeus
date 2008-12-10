using System;
using System.Web.UI.WebControls;
using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Sources;
using SoundInTheory.DynamicImage.Filters;
using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class ImageEditor : FileUpload
	{
		private CheckBox chkClearImage;

		public byte[] Image
		{
			get;
			set;
		}

		public bool ClearImage
		{
			get
			{
				return (chkClearImage != null) ? chkClearImage.Checked : false;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			EnsureChildControls();
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			this.Controls.Add(new LiteralControl("<br />"));

			if (this.Image != null)
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
							new BytesImageSource { Bytes = this.Image }
						},
						Filters = new FilterCollection
						{
							new ResizeFilter { Width = 200, Height = 200 }
						}
					}
				}
				});

				this.Controls.Add(new LiteralControl("<br />"));

				chkClearImage = new CheckBox { ID = "chkClearImage", Text = "Clear", CssClass = "clearFile" };
				this.Controls.Add(chkClearImage);
			}
		}
	}
}
