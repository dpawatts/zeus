using System.Collections.Generic;
using SoundInTheory.DynamicImage.Caching;
using Zeus.ContentProperties;

namespace Zeus.FileSystem.Images
{
	public class ZeusImageDataInCollectionSource : ZeusImageDataSource
	{
		#region Properties

		public int Index
		{
			get { return (int) (ViewState["Index"] ?? 0); }
			set { ViewState["Index"] = value; }
		}

		#endregion

		protected override ImageData GetImageData(ContentItem contentItem)
		{
			PropertyCollection detailCollection = contentItem.GetDetailCollection(DetailName, false);
			if (detailCollection != null && detailCollection.Count > Index)
				return detailCollection[Index] as ImageData;
			return null;
		}

		public override void PopulateDependencies(List<Dependency> dependencies)
		{
			base.PopulateDependencies(dependencies);
			dependencies.Add(new Dependency {Text1 = ContentID.ToString(), Text2 = DetailName, Text3 = Index.ToString()});
		}
	}
}