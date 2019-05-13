using Ninject;
using SoundInTheory.DynamicImage.Caching;
using Zeus.Persistence;
using Zeus.Globalization.ContentTypes;
using System.Collections;
using System.Collections.Generic;

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
                List<string> keysToRemove = new List<string>();
                IDictionaryEnumerator enumerator = System.Web.HttpContext.Current.Cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string key = (string)enumerator.Key;
                    if (key.StartsWith("primaryNav"))
                    {
                        keysToRemove.Add(key);
                    }
                }

                foreach (string key in keysToRemove)
                {
                    System.Web.HttpContext.Current.Cache.Remove(key);
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