using System.Linq;
using Ninject;
using SoundInTheory.DynamicImage.Caching;
using Zeus.ContentProperties;
using Zeus.Persistence;

namespace Zeus.FileSystem.Images
{
	public class ImageCachingService : IStartable
	{
		private readonly IPersister _persister;

		public ImageCachingService(IPersister persister)
		{
			_persister = persister;
		}

		public void Start()
		{
			_persister.ItemDeleted += OnPersisterItemDeleted;
			_persister.ItemSaved += OnPersisterItemSaved;
		}

		private static void OnPersisterItemDeleted(object sender, ItemEventArgs e)
		{
			DeleteCachedImages(e.AffectedItem);
		}

		private static void OnPersisterItemSaved(object sender, ItemEventArgs e)
		{
			DeleteCachedImages(e.AffectedItem);
		}

		private static void DeleteCachedImages(ContentItem contentItem)
		{
			if (DynamicImageCacheManager.Enabled)
			{
				if (contentItem is Image)
				{
					ZeusImageSource source = new ZeusImageSource { ContentID = contentItem.ID };
					DynamicImageCacheManager.Remove(source);
				}
				else
				{
					foreach (ImageDataProperty imageDataProperty in contentItem.Details.OfType<ImageDataProperty>())
					{
						ZeusImageDataSource source = new ZeusImageDataSource { ContentID = contentItem.ID, DetailName = imageDataProperty.Name };
						DynamicImageCacheManager.Remove(source);
					}

					foreach (PropertyCollection propertyCollection in contentItem.DetailCollections.Values)
					{
						var imageDataProperties = propertyCollection.Details.OfType<ImageDataProperty>(); 
						for (int i = 0, length = imageDataProperties.Count(); i < length; ++i)
						{
							ImageDataProperty imageDataProperty = imageDataProperties.ElementAt(i);
							ZeusImageDataInCollectionSource source = new ZeusImageDataInCollectionSource { ContentID = contentItem.ID, DetailName = imageDataProperty.Name, Index = i };
							DynamicImageCacheManager.Remove(source);
						}
					}
				}
			}
		}

		public void Stop()
		{
			_persister.ItemDeleted -= OnPersisterItemDeleted;
			_persister.ItemSaved -= OnPersisterItemSaved;
		}
	}
}