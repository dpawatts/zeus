using System.Web;

namespace Zeus.AddIns.AntiSpam.Services
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
		/// <returns></returns>
		void Check(HttpContextBase httpContext);
	}
}