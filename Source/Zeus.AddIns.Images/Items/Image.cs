using System;
using Zeus.FileSystem;
using SoundInTheory.DynamicImage.Caching;
using Zeus.ContentTypes.Properties;
using System.Collections.Generic;

namespace Zeus.AddIns.Images.Items
{
	public class Image : File
	{
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
			if (contentItem is Image)
			{
				ZeusImageSource source = new ZeusImageSource { ContentID = contentItem.ID };
				DynamicImageCacheManager.Remove(source);
			}
		}
	}
}
