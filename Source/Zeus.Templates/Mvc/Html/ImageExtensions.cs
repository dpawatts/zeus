using System.Web.Mvc;
using SoundInTheory.DynamicImage.Fluent;
using Zeus.FileSystem.Images;

namespace Zeus.Templates.Mvc.Html
{
	public static class ImageExtensions
	{
		public static string ImageUrl(this HtmlHelper helper, Image image, int width, int height, bool fill)
		{
			if (image == null)
				return string.Empty;
			return image.GetUrl(width, height, fill);
		}

		public static string ImageUrl(this HtmlHelper helper, Image image, int width, int height)
		{
			if (image == null)
				return string.Empty;
			return image.GetUrl(width, height);
		}

		public static string ImageUrl(this HtmlHelper helper, Image image)
		{
			if (image == null)
				return string.Empty;
			return new DynamicImageBuilder()
				.WithLayer(LayerBuilder.Image.SourceImage(image))
				.Url;
		}

		public static string Image(this HtmlHelper helper, Image image, int width, int height, bool fill)
		{
			if (image == null)
				return string.Empty;
			return ImageTag(helper, image, ImageUrl(helper, image, width, height, fill));
		}

		public static string Image(this HtmlHelper helper, Image image, int width, int height)
		{
			if (image == null)
				return string.Empty;
			return ImageTag(helper, image, ImageUrl(helper, image, width, height));
		}

		public static string Image(this HtmlHelper helper, Image image)
		{
			if (image == null)
				return string.Empty;
			return ImageTag(helper, image, ImageUrl(helper, image));
		}

		public static string ImageTag(this HtmlHelper helper, Image image, string imageUrl)
		{
			TagBuilder imageTag = new TagBuilder("img");
			imageTag.MergeAttribute("src", imageUrl, true);
			imageTag.MergeAttribute("alt", image.Caption, true);
			imageTag.MergeAttribute("border", "0", true);
			return imageTag.ToString();
		}
	}
}