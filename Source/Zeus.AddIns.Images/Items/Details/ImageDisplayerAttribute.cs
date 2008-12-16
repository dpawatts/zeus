using System;
using Zeus.ContentTypes.Properties;
using System.Web.UI;
using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Sources;
using SoundInTheory.DynamicImage.Filters;

namespace Zeus.AddIns.Images.Items.Details
{
	public class ImageDisplayerAttribute : DisplayerAttribute
	{
		public int Width
		{
			get;
			set;
		}

		public int Height
		{
			get;
			set;
		}

		public ResizeMode ResizeMode
		{
			get;
			set;
		}

		public ImageDisplayerAttribute()
		{
			this.ResizeMode = ResizeMode.Uniform;
		}

		public override Control AddTo(Control container, ContentItem item, string propertyName)
		{
			DynamicImage dynamicImage = null;
			if (item[propertyName] as Image != null)
			{
				dynamicImage = new DynamicImage
				{
					CssClass = "image",
					Layers = new LayerCollection
					{
						new ImageLayer
						{
							Source = new ImageSourceCollection
							{
								new ZeusImageSource { ContentID = ((Image) item[propertyName]).ID }
							},
							Filters = new FilterCollection
							{
								new ResizeFilter { Width = this.Width, Height = this.Height, Mode = this.ResizeMode }
							}
						}
					}
				};
				container.Controls.Add(dynamicImage);
			}
			return dynamicImage;
		}
	}
}
