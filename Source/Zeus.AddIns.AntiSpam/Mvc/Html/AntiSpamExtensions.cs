using System.Web.Mvc;
using Zeus.AddIns.AntiSpam.Services;

namespace Zeus.AddIns.AntiSpam.Mvc.Html
{
	public static class AntiSpamExtensions
	{
		public static string Captcha(this HtmlHelper helper, string error)
		{
			return Context.Current.Resolve<ICaptchaService>().GetClientHtml(error);
		}
	}
}