using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Isis.ExtensionMethods.IO;
using Isis.Web;
using SoundInTheory.DynamicImage.Caching;
using Zeus.ContentProperties;

namespace Zeus.FileSystem.Images
{
	[Serializable]
	public class ImageData : FileData
	{
		#region Static caching methods

		static ImageData()
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
			if (DynamicImageCacheManager.Enabled)
			{
				ProcessDetails(contentItem.Details.Values);
				foreach (PropertyCollection detailCollection in contentItem.DetailCollections.Values)
					ProcessDetails(detailCollection.Details);
			}
		}

		private static void ProcessDetails(IEnumerable<PropertyData> details)
		{
			foreach (ObjectProperty objectDetail in details.OfType<ObjectProperty>().Where(od => od.Value != null && od.Value is ImageData))
			{
				ZeusImageDataSource source = new ZeusImageDataSource { ContentID = objectDetail.EnclosingItem.ID, DetailName = objectDetail.Name };
				DynamicImageCacheManager.Remove(source);
			}
		}

		#endregion

		public static ImageData FromStream(Stream stream, string filename)
		{
			byte[] fileBytes = stream.ReadAllBytes();
			ImageData imageData = new ImageData
    	{
				ContentType = MimeUtility.GetMimeType(fileBytes),
				Data = fileBytes,
    		FileName = filename,
    		Size = stream.Length
    	};
			return imageData;
		}
	}
}