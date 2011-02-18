using System.IO;
using SoundInTheory.DynamicImage.Fluent;
using Zeus.BaseLibrary.ExtensionMethods.IO;
using Zeus.BaseLibrary.Web;
using Zeus.Design.Editors;
using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Filters;

namespace Zeus.FileSystem.Images
{
	[ContentType]
	public class Image : File
	{
		public Image()
		{
			base.Visible = false;
		}
		
		[ImageUploadEditor("Image", 100)]
		public override byte[] Data
		{
			get { return base.Data; }
			set { base.Data = value; }
		}

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

		public string GetUrl(int width, int height, bool fill, DynamicImageFormat format)
		{
            DynamicImage image = new DynamicImage();
            image.ImageFormat = format;
            ImageLayer imageLayer = new ImageLayer();

            ZeusImageSource source = new ZeusImageSource();
            source.ContentID = this.ID;

            imageLayer.Source.SingleSource = source;

            ResizeFilter resizeFilter = new ResizeFilter();
		    resizeFilter.Mode = fill ? ResizeMode.UniformFill : ResizeMode.Uniform;
		    resizeFilter.Width = SoundInTheory.DynamicImage.Unit.Pixel(width);
		    resizeFilter.Height = SoundInTheory.DynamicImage.Unit.Pixel(height);

            imageLayer.Filters.Add(resizeFilter);
            
		    image.Layers.Add(imageLayer);                

			return image.ImageUrl;

            /*old code replaced
             * 
            return new DynamicImageBuilder()
				.WithLayer(
					LayerBuilder.Image.SourceImage(this).WithFilter(FilterBuilder.Resize.To(width, height, fill)))
				.Url;
             */
		}

        public string GetUrl(int width, int height, bool fill)
		{
            return GetUrl(width, height, fill, DynamicImageFormat.Jpeg);
		}

		public string GetUrl(int width, int height)
		{
            return GetUrl(width, height, true, DynamicImageFormat.Jpeg);
		}
	}
}