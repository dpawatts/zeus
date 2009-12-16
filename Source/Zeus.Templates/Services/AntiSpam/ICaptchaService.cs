using System.Web;

namespace Zeus.Templates.Services.AntiSpam
{
	public interface ICaptchaService
	{
		/// <summary>
		/// Returns any HTML that needs to be rendered on the client; for example,
		/// RECAPTCHA uses this to render some javascript, which in turns renders
		/// the CAPTCHA image.
		/// </summary>
		/// <returns></returns>
		string GetClientHtml(string error);

		/// <summary>
		/// Implemented by each CAPTCHA service to do the actual check.
		/// </summary>
		/// <param name="httpContext"></param>
		/// <param name="error"></param>
		/// <returns></returns>
		bool Check(HttpContextBase httpContext, out string error);
	}
}