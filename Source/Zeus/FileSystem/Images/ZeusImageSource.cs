using System.Collections.Generic;
using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Caching;
using SoundInTheory.DynamicImage.Sources;
using SoundInTheory.DynamicImage.Util;

namespace Zeus.FileSystem.Images
{
	public class ZeusImageSource : ImageSource
	{
		#region Properties

		public int ContentID
		{
			get { return (int) (this["ContentID"] ?? 0); }
			set { this["ContentID"] = value; }
		}

		public bool ThrowExceptionOnFormatError
		{
			get { return (bool)(this["ThrowExceptionOnFormatError"] ?? false); }
			set { this["ThrowExceptionOnFormatError"] = value; }
		}

		#endregion

		public override FastBitmap GetBitmap()
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