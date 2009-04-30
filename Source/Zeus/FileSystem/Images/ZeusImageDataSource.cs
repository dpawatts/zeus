using System.ComponentModel;
using SoundInTheory.DynamicImage.Util;
using SoundInTheory.DynamicImage.Sources;
using System.Collections.Generic;
using SoundInTheory.DynamicImage.Caching;

namespace Zeus.FileSystem.Images
{
	public class ZeusImageDataSource : ImageSource
	{
		#region Properties

		public int ContentID
		{
			get { return (int) (ViewState["ContentID"] ?? 0); }
			set { ViewState["ContentID"] = value; }
		}

		public string DetailName
		{
			get { return ViewState["DetailName"] as string ?? string.Empty; }
			set { ViewState["DetailName"] = value; }
		}

		#endregion

		public override FastBitmap GetBitmap(ISite site, bool designMode)
		{
			ContentItem contentItem = Context.Persister.Get(ContentID);
			ImageData imageData = GetImageData(contentItem);
			return (imageData != null && imageData.Data != null) ? new FastBitmap(imageData.Data) : null;
		}

		protected virtual ImageData GetImageData(ContentItem contentItem)
		{
			return contentItem[DetailName] as ImageData;
		}

		public override void PopulateDependencies(List<Dependency> dependencies)
		{
			base.PopulateDependencies(dependencies);
			dependencies.Add(new Dependency { Text1 = ContentID.ToString(), Text2 = DetailName });
		}
	}
}