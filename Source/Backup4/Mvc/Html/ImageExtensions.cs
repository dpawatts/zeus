using System.Web.Mvc;
using SoundInTheory.DynamicImage.Fluent;
using Zeus.FileSystem.Images;
using SoundInTheory.DynamicImage;

namespace Zeus.Templates.Mvc.Html
{
	public static class ImageExtensions
	{
        /// <summary>
        /// Image methods
        /// </summary>

        public static string Image(this HtmlHelper helper, Image image)
        {
            return Image(helper, image, 0, 0);
        }

        public static string Image(this HtmlHelper helper, Image image, int width, int height)
        {
            return Image(helper, image, width, height, true);
        }

        public static string Image(this HtmlHelper helper, Image image, int width, int height, bool fill)
        {
            return Image(helper, image, width, height, fill, string.Empty);
        }

        public static string Image(this HtmlHelper helper, Image image, int width, int height, bool fill, DynamicImageFormat format)
        {
            return Image(helper, image, width, height, fill, string.Empty, format);
        }

        public static string Image(this HtmlHelper helper, Image image, int width, int height, bool fill, string defaultImage)
        {
            return Image(helper, image, width, height, fill, defaultImage, DynamicImageFormat.Jpeg);
        }

        public static string Image(this HtmlHelper helper, Image image, int width, int height, bool fill, string defaultImage, DynamicImageFormat format)
        {
            string url = ImageUrl(helper, image, width, height, fill, defaultImage, format);
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            return ImageTag(helper, image, url);
        }

        /// <summary>
        /// Image URL methods
        /// </summary>
        
        public static string ImageUrl(this HtmlHelper helper, Image image)
        {
            return ImageUrl(helper, image, 0, 0);
        }

        public static string ImageUrl(this HtmlHelper helper, Image image, int width, int height)
		{
            return ImageUrl(helper, image, width, height, true);
		}

        public static string ImageUrl(this HtmlHelper helper, Image image, int width, int height, bool fill)
        {
            return ImageUrl(helper, image, width, height, fill, string.Empty, DynamicImageFormat.Jpeg);
        }

        public static string ImageUrl(this HtmlHelper helper, Image image, int width, int height, bool fill, DynamicImageFormat format)
        {
            return ImageUrl(helper, image, width, height, fill, string.Empty, format);
        }

        public static string ImageUrl(this HtmlHelper helper, Image image, int width, int height, bool fill, string defaultImage, DynamicImageFormat format)
        {
            
            string result = defaultImage;

            // only generate url if image exists
            if (image != null)
            {
                // special code for image without resizing
                if (width == 0 && height == 0)
                {
                    result = new CompositionBuilder()
                        .WithLayer(LayerBuilder.Image.SourceImage(image))
                        .Url;
                }

                // generate resized image url
                else
                {
                    if (image is CroppedImage)
                    {
                        CroppedImage cImage = (CroppedImage)image;
                        result = cImage.GetUrl(width, height, fill, format);
                    }
                    else
                    {
                        result = image.GetUrl(width, height, fill, format);
                    }
                }
            }

            return result;
            
        }

        /// <summary>
        /// Image Tag methods
        /// </summary>

		public static string ImageTag(this HtmlHelper helper, Image image, string imageUrl)
		{
            TagBuilder imageTag = new TagBuilder("img");

            imageTag.MergeAttribute("src", imageUrl, true);
            imageTag.MergeAttribute("alt", image != null ? image.Caption : string.Empty, true);
            imageTag.MergeAttribute("border", "0", true);

            return imageTag.ToString(TagRenderMode.SelfClosing);
        }
	}
}