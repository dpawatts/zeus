using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Filters;
using SoundInTheory.DynamicImage.Sources;
using SoundInTheory.DynamicImage.Caching;

namespace Zeus.FileSystem.Images
{
	public class Image : File
	{
		#region Static caching methods

		static Image()
		{
			Context.Persister.ItemDeleted += Persister_ItemDeleted;
			Context.Persister.ItemSaved += Persister_ItemSaved;
		}

		private static void Persister_ItemDeleted(object sender, ItemEventArgs e)
		{
			DeleteCache(e.AffectedItem);
		}

		private static void Persister_ItemSaved(object sender, ItemEventArgs e)
		{
			DeleteCache(e.AffectedItem);
		}

		private static void DeleteCache(ContentItem contentItem)
		{
			if (contentItem is Image && DynamicImageCacheManager.Enabled)
			{
				ZeusImageSource source = new ZeusImageSource { ContentID = contentItem.ID };
				DynamicImageCacheManager.Remove(source);
			}
		}

		#endregion

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