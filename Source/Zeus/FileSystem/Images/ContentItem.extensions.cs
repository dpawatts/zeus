using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Filters;
using SoundInTheory.DynamicImage.Sources;

namespace Zeus.FileSystem.Images
{
	public static class ContentItemExtensionMethods
	{
		public static string GetImageUrl(this ContentItem contentItem, string detailName, int width, int height, bool fill)
		{
			ZeusImageDataSource imageSource = new ZeusImageDataSource
			                                  	{
			                                  		ContentID = contentItem.ID,
			                                  		DetailName = detailName
			                                  	};
			return GetImageUrl(imageSource, width, height, fill);
		}

		public static string GetImageUrl(this ContentItem contentItem, string detailCollectionName, int index, int width, int height, bool fill)
		{
			ZeusImageDataInCollectionSource imageSource = new ZeusImageDataInCollectionSource
			                                              	{
			                                              		ContentID = contentItem.ID,
			                                              		DetailName = detailCollectionName,
			                                              		Index = index
			                                              	};
			return GetImageUrl(imageSource, width, height, fill);
		}

		public static string GetImageUrl(this ContentItem contentItem, string detailName)
		{
			ZeusImageDataSource imageSource = new ZeusImageDataSource
			                                  	{
			                                  		ContentID = contentItem.ID,
			                                  		DetailName = detailName
			                                  	};
			ImageLayer imageLayer = GetImageLayer(imageSource);
			return new DynamicImage { Layers = new LayerCollection { imageLayer } }.ImageUrl;
		}

		internal static string GetImageUrl(ImageSource imageSource, int width, int height, bool fill)
		{
			ImageLayer imageLayer = GetImageLayer(imageSource);
			imageLayer.Filters.Add(new ResizeFilter
			                       	{
			                       		Width = Unit.Pixel(width),
			                       		Height = Unit.Pixel(height),
			                       		Mode = (fill) ? ResizeMode.UniformFill : ResizeMode.Uniform,
			                       		EnlargeImage = true
			                       	});
			return new DynamicImage { Layers = new LayerCollection { imageLayer } }.ImageUrl;
		}

		private static ImageLayer GetImageLayer(ImageSource imageSource)
		{
			return new ImageLayer
			       	{
			       		Source = new ImageSourceCollection
			       		         	{
			       		         		SingleSource = imageSource
			       		         	}
			       	};
		}
	}
}