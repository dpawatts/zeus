using System.Web.Mvc;
using SoundInTheory.DynamicImage.Fluent;
using Zeus.FileSystem.Images;

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

        public static string Image(this HtmlHelper helper, Image image, int width, int height, bool fill, string defaultImage)
        {
            string url = ImageUrl(helper, image, width, height, fill, defaultImage);
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
            return ImageUrl(helper, image, width, height, fill, string.Empty);
        }

        public static string ImageUrl(this HtmlHelper helper, Image image, int width, int height, bool fill, string defaultImage)
        {
            string result = defaultImage;

            // only generate url if image exists
            if (image != null)
            {
                // special code for image without resizing
                if (width == 0 && height == 0)
                {
                    result = new DynamicImageBuilder()
                        .WithLayer(LayerBuilder.Image.SourceImage(image))
                        .Url;
                }

                // generate resized image url
                else
                {
                    result = image.GetUrl(width, height, fill);
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