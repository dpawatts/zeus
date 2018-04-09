using System.IO;
using SoundInTheory.DynamicImage.Fluent;
using Zeus.BaseLibrary.ExtensionMethods.IO;
using Zeus.BaseLibrary.Web;
using Zeus.Design.Editors;
using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Filters;
using System.Drawing;
using System;

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
            string appKey = "ZeusImage_" + this.ID + "_" + width + "_" + height + "_" + fill.ToString();
            string res = System.Web.HttpContext.Current.Cache[appKey] == null ? null : System.Web.HttpContext.Current.Cache[appKey].ToString();
            DateTime lastUpdated = res != null ? (DateTime)System.Web.HttpContext.Current.Cache[appKey + "_timer"] : DateTime.MinValue;

            if (res != null && lastUpdated == this.Updated)
            {
                return res;
            }

            DynamicImage image = new DynamicImage();
            image.Fill.BackgroundColour = Color.White;

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

			string url = image.ImageUrl;

            /*old code replaced
             * 
            return new DynamicImageBuilder()
				.WithLayer(
					LayerBuilder.Image.SourceImage(this).WithFilter(FilterBuilder.Resize.To(width, height, fill)))
				.Url;
             */

            System.Web.HttpContext.Current.Cache[appKey] = url;
            System.Web.HttpContext.Current.Cache[appKey + "_timer"] = this.Updated;

            return url;
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