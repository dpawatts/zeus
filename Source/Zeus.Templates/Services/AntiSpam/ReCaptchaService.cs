using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using Zeus.Templates.Configuration;

namespace Zeus.Templates.Services.AntiSpam
{
	/// <summary>
	/// The API is documented at http://recaptcha.net/apidocs/captcha/.
	/// </summary>
	public class ReCaptchaService : ICaptchaService
	{
		public readonly ReCaptchaElement _configuration;

		public ReCaptchaService(ReCaptchaElement configuration)
		{
			_configuration = configuration;
		}

		public string GetClientHtml(string error)
		{
			return
				string.Format(
					@"<script type=""text/javascript""
   src=""http://api.recaptcha.net/challenge?k={0}&error={1}"">
</script>

<noscript>
   <iframe src=""http://api.recaptcha.net/noscript?k={0}&error={1}""
       height=""300"" width=""500"" frameborder=""0""></iframe><br>
   <textarea name=""recaptcha_challenge_field"" rows=""3"" cols=""40"">
   </textarea>
   <input type=""hidden"" name=""recaptcha_response_field"" 
       value=""manual_challenge"">
</noscript>

<br />
", _configuration.PublicKey, error);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="CaptchaException" />
		/// <param name="httpContext"></param>
		/// <param name="error"></param>
		public bool Check(HttpContextBase httpContext, out string error)
		{
			using (WebClient webClient = new WebClient())
			{
				NameValueCollection values = new NameValueCollection();

				// Your private key.
				values["privatekey"] = _configuration.PrivateKey;

				// The IP address of the user who solved the CAPTCHA. 
				values["remoteip"] = httpContext.Request.UserHostAddress;

				// The value of "recaptcha_challenge_field" sent via the form.
				values["challenge"] = httpContext.Request["recaptcha_challenge_field"];

				// The value of "recaptcha_response_field" sent via the form.
				values["response"] = httpContext.Request["recaptcha_response_field"];

				byte[] responseBytes = webClient.UploadValues(
					"http://api-verify.recaptcha.net/verify",
					values);

				// The response from verify is a series of strings separated by \n.
				// To read the string, split the line and read each field.
				// New lines may be added in the future.
				// Implementations should ignore these lines.
				string response = webClient.Encoding.GetString(responseBytes);

				string[] responseLines = response.Split(new[] { '\n' }, StringSplitOptions.None);

				// Line 1 - "true" or "false". True if the reCAPTCHA was successful.
				bool success = Convert.ToBoolean(responseLines[0]);

				error = null;
				if (success)
					return true;

				// Line 2 - If Line 1 is false, then this string will be an error code.
				// reCAPTCHA can display the error to the user (through the error parameter
				// of api.recaptcha.net/challenge). Implementations should not depend on
				// error code names, as they may change in the future. 
				error = responseLines[1];
				return false;
			}
		}
	}
}