using Ninject;
using SoundInTheory.DynamicImage.Caching;
using Zeus.Persistence;
using Zeus.Globalization.ContentTypes;

namespace Zeus.Admin.NavigationFlag
{
	public class NavigationCachingService : IStartable
	{
		private readonly IPersister _persister;

        public NavigationCachingService(IPersister persister)
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
            //any time anything is saved or changed, delete all the primary nav app cache data
            if (contentItem.IsPage)
            {
                foreach (string item in System.Web.HttpContext.Current.Application.AllKeys)
                {
                    if (item.StartsWith("primaryNav"))
                    {
                        System.Web.HttpContext.Current.Application.Remove(item);
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