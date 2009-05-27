using System.ComponentModel;
using SoundInTheory.DynamicImage;
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

		public bool ThrowExceptionOnFormatError
		{
			get { return (bool)(ViewState["ThrowExceptionOnFormatError"] ?? false); }
			set { ViewState["ThrowExceptionOnFormatError"] = value; }
		}

		#endregion

		public override FastBitmap GetBitmap(ISite site, bool designMode)
		{
			Image image = Context.Persister.Get(ContentID) as Image;
			if (image != null && image.Data != null)
			{
				try
				{
					return new FastBitmap(image.Data);
				}
				catch (DynamicImageException ex)
				{
					if (ThrowExceptionOnFormatError)
						throw new ZeusException("Could not load image from file with file extension '" + image.FileExtension + "' and content type '" + image.ContentType + "'", ex);
					return null;
				}
			}
			return null;
		}

		public override void PopulateDependencies(List<Dependency> dependencies)
		{
			dependencies.Add(new Dependency { Text1 = ContentID.ToString() });
		}
	}
}