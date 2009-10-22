using System.Net;
using Zeus.BaseLibrary.Web;

namespace Zeus.Net
{
	public interface IHttpClient
	{
		/// <summary>
		/// Posts the request and returns a text response.  
		/// This is all that is needed for Akismet.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="userAgent">The user agent.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="formParameters">The properly formatted parameters.</param>
		/// <returns></returns>
		string PostRequest(Url url, string userAgent, int timeout, string formParameters);

		/// <summary>
		/// Posts the request.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="userAgent">The user agent.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="formParameters">The form parameters.</param>
		/// <param name="proxy">The proxy.</param>
		/// <returns></returns>
		string PostRequest(Url url, string userAgent, int timeout, string formParameters, IWebProxy proxy);
	}
}