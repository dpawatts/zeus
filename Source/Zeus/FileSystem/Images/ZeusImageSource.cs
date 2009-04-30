using System.ComponentModel;
using SoundInTheory.DynamicImage.Sources;
using System.Collections.Generic;
using SoundInTheory.DynamicImage.Caching;
using SoundInTheory.DynamicImage.Util;

namespace Zeus.FileSystem.Images
{
	public class ZeusImageSource : ImageSource
	{
		#region Properties

		public int ContentID
		{
			get { return (int) (ViewState["ContentID"] ?? 0); }
			set { ViewState["ContentID"] = value; }
		}

		#endregion

		public override FastBitmap GetBitmap(ISite site, bool designMode)
		{
			Image image = Context.Persister.Get(ContentID) as Image;
			return (image != null && image.Data != null) ? new FastBitmap(image.Data) : null;
		}

		public override void PopulateDependencies(List<Dependency> dependencies)
		{
			base.PopulateDependencies(dependencies);
			dependencies.Add(new Dependency { Text1 = ContentID.ToString() });
		}
	}
}