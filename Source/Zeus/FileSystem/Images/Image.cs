using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Filters;
using SoundInTheory.DynamicImage.Sources;
using SoundInTheory.DynamicImage.Caching;

namespace Zeus.FileSystem.Images
{
	public class Image : File
	{
		#region Methods

		public string GetUrl(int width, int height, bool fill)
		{
			return new DynamicImage
			{
				Layers = new LayerCollection
				{
					new ImageLayer
					{
						Source = new ImageSourceCollection
						{
							SingleSource = new ZeusImageSource
							{
								ContentID = ID
							}
						},
						Filters = new FilterCollection
						{
							new ResizeFilter
							{
								Width = Unit.Pixel(width),
								Height = Unit.Pixel(height),
								Mode = (fill) ? ResizeMode.UniformFill : ResizeMode.Uniform,
								EnlargeImage = true
							}
						}
					}
				},
			}.ImageUrl;
		}

		#endregion
	}
}