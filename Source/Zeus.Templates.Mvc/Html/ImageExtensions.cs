using System.Web.Mvc;
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
			return image.Url;
		}

		public static string Image(this HtmlHelper helper, Image image, int width, int height, bool fill)
		{
			if (image == null)
				return string.Empty;
			return ImageTag(image, ImageUrl(helper, image, width, height, fill));
		}

		public static string Image(this HtmlHelper helper, Image image, int width, int height)
		{
			if (image == null)
				return string.Empty;
			return ImageTag(image, ImageUrl(helper, image, width, height));
		}

		public static string Image(this HtmlHelper helper, Image image)
		{
			if (image == null)
				return string.Empty;
			return ImageTag(image, ImageUrl(helper, image));
		}

		private static string ImageTag(Image image, string imageUrl)
		{
			MvcContrib.UI.Tags.Image imageTag = new MvcContrib.UI.Tags.Image
			{
				Src = imageUrl,
				Alt = image.Caption
			};
			imageTag.Attributes["border"] = "0";
			return imageTag.ToString();
		}
	}
}