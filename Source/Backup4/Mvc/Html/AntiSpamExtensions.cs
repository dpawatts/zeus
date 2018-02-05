using System.Web.Mvc;
using Zeus.Templates.Services.AntiSpam;

namespace Zeus.Templates.Mvc.Html
{
	public static class AntiSpamExtensions
	{
		public static string Captcha(this HtmlHelper helper, string error)
		{
			return Context.Current.Resolve<ICaptchaService>().GetClientHtml(error);
		}
	}
}