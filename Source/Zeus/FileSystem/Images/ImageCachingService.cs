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
			}
		}

		public void Stop()
		{
			_persister.ItemDeleted -= OnPersisterItemDeleted;
			_persister.ItemSaved -= OnPersisterItemSaved;
		}
	}
}