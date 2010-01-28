using Ninject;
using SoundInTheory.DynamicImage.Caching;
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

		private void OnPersisterItemDeleted(object sender, ItemEventArgs e)
		{
			DeleteCachedImages(e.AffectedItem);
		}

		private void OnPersisterItemSaved(object sender, ItemEventArgs e)
		{
			DeleteCachedImages(e.AffectedItem);
		}

		public void DeleteCachedImages(ContentItem contentItem)
		{
			if (contentItem is Image)
			{
				var source = new ZeusImageSource { ContentID = contentItem.ID };
				DynamicImageCacheManager.Remove(source);
			}
		}

		public void Stop()
		{
			_persister.ItemDeleted -= OnPersisterItemDeleted;
			_persister.ItemSaved -= OnPersisterItemSaved;
		}
	}
}