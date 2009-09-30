using System.Net;
using System.Text.RegularExpressions;
using Isis.Net;
using Isis.Web;

namespace Zeus.AddIns.Blogs.Services.Tracking
{
	/// <summary>
	/// Used to verify that a trackback or pingback source actually contains a link to this site.
	/// </summary>
	public static class Verifier
	{
		/// <summary>
		/// Checks that the contents of the source url contains the target URL.
		/// </summary>
		/// <param name="sourceUrl">The source URL.</param>
		/// <param name="targetUrl">The target URL.</param>
		/// <param name="pageTitle">The page title.</param>
		/// <returns></returns>
		public static bool SourceContainsTarget(Url sourceUrl, Url targetUrl, out string pageTitle)
		{
			pageTitle = string.Empty;
			string page = null;
			try
			{
				page = WebClientUtility.GetHtml(sourceUrl);
			}
			catch (WebException)
			{
				//Log.Warn("Could not verify the source of a ping/trackback", e);
			}
			if (page == null || targetUrl == null)
			{
				return false;
			}

			string pat = @"<head.*?>.*<title.*?>(.*)</title.*?>.*</head.*?>";
			var reg = new Regex(pat, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Match m = reg.Match(page);
			if (m.Success)
			{
				pageTitle = m.Result("$1");
				return true;
			}
			return false;
		}

		/// <summary>
		/// Checks that the contents of the source url contains the target URL.
		/// </summary>
		/// <param name="sourceUrl">The source URL.</param>
		/// <param name="targetUrl">The target URL.</param>
		/// <returns></returns>
		public static bool SourceContainsTarget(Url sourceUrl, Url targetUrl)
		{
			string page;
			return SourceContainsTarget(sourceUrl, targetUrl, out page);
		}
	}
}