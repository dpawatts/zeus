using System.IO;
using SoundInTheory.DynamicImage.Caching;
using SoundInTheory.DynamicImage.Layers;
using Zeus.BaseLibrary.ExtensionMethods;
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
				ContentType = fileBytes.GetMimeType(),
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

            Composition image = new Composition();
            image.ImageFormat = format;
            ImageLayer imageLayer = new ImageLayer();
            
            ZeusImageSource source = new ZeusImageSource();
            source.ContentID = ID;

            imageLayer.Source = source;

            ResizeFilter resizeFilter = new ResizeFilter();
		    resizeFilter.Mode = fill ? ResizeMode.UniformFill : ResizeMode.Uniform;
		    resizeFilter.Width = Unit.Pixel(width);
		    resizeFilter.Height = Unit.Pixel(height);

            imageLayer.Filters.Add(resizeFilter);
            
            imageLayer.Filters.Add(resizeFilter);

            string url = ImageUrlGenerator.GetImageUrl(image);

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