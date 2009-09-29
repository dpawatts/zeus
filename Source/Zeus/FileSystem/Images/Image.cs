using System.IO;
using Isis.ExtensionMethods.IO;
using Isis.Web;
using SoundInTheory.DynamicImage.Fluent;
using Zeus.Design.Editors;

namespace Zeus.FileSystem.Images
{
	[ContentType]
	public class Image : File
	{
		[ImageUploadEditor("Image", 100)]
		public override byte[] Data
		{
			get { return base.Data; }
			set { base.Data = value; }
		}

		/*public override string Url
		{
			get
			{
				return new DynamicImageBuilder()
					.WithLayer(LayerBuilder.Image.SourceImage(this))
					.Url;
			}
		}*/

		public static Image FromStream(Stream stream, string filename)
		{
			byte[] fileBytes = stream.ReadAllBytes();
			return new Image
			{
				ContentType = MimeUtility.GetMimeType(fileBytes),
				Data = fileBytes,
				Name = filename,
				Size = stream.Length
			};
		}

		public string GetUrl(int width, int height, bool fill)
		{
			return new DynamicImageBuilder()
				.WithLayer(
					LayerBuilder.Image.SourceImage(this).WithFilter(FilterBuilder.Resize.To(width, height, fill)))
				.Url;
		}

		public string GetUrl(int width, int height)
		{
			return GetUrl(width, height, true);
		}
	}
}