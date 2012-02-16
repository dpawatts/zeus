using System;
using System.Web.UI;
using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Sources;
using SoundInTheory.DynamicImage.Filters;

namespace Zeus.Design.Displayers
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

		public override void InstantiateIn(Control container)
		{
			
		}

		public override void SetValue(Control container, ContentItem item, string propertyName)
		{
			/*if (item[propertyName] as Image != null)
			{
				DynamicImage dynamicImage = new DynamicImage
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
								new ResizeFilter { Width = Unit.Pixel(Width), Height = Unit.Pixel(Height), Mode = ResizeMode }
							}
						}
					}
				};
				container.Controls.Add(dynamicImage);
			}*/
		}
	}
}