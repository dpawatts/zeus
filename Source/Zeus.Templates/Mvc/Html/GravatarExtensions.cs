using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Zeus.Templates.Mvc.Html
{
	/// <summary>
	/// Implements methods that allow access to Gravatar images.
	/// See http://en.gravatar.com/site/implement/url for reference.
	/// </summary>
	public static class GravatarExtensions
	{
		public static string GravatarImageUrl(this HtmlHelper helper, int size, string email)
		{
			return GravatarImageUrl(helper, size, email, GravatarDefaultType.Identicon);
		}

		public static string GravatarImageUrl(this HtmlHelper helper, int size, string email, GravatarDefaultType defaultImage)
		{
			if (string.IsNullOrEmpty(email))
				email = "None";

			string emailForUrl = email.ToLowerInvariant().Replace(" ", string.Empty);
			emailForUrl = (FormsAuthentication.HashPasswordForStoringInConfigFile(emailForUrl, "md5") ?? string.Empty)
				.ToLowerInvariant();

			emailForUrl = HttpUtility.UrlEncode(emailForUrl);

			return string.Format(CultureInfo.InvariantCulture,
				"http://www.gravatar.com/avatar/{0}.jpg?s={1}&amp;default={2}",
				emailForUrl, size, defaultImage.ToString().ToLower());
		}
	}

	public enum GravatarDefaultType
	{
		Identicon,
		MonsterID,
		Wavatar
	}
}