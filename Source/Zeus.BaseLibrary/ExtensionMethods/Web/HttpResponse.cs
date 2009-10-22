using System.Web;
using Zeus.BaseLibrary.Web;

namespace Zeus.BaseLibrary.ExtensionMethods.Web
{
	public static class HttpResponseExtensionMethods
	{
		/// <summary>
		/// Writes an HTML line break to the HTTP response.
		/// </summary>
		/// <param name="response"></param>
		public static void WriteLine(this HttpResponse response)
		{
			response.Write("<br />");
		}

		/// <summary>
		/// Writes a variable name and value, followed by an HTML line break, to the HTTP response.
		/// </summary>
		/// <param name="response"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public static void WriteLine(this HttpResponse response, string name, object value)
		{
			response.Write(name + " = " + (value ?? "null") + "<br />");
		}

		public static void Redirect(this HttpResponse response, string url, SecureQueryString qs)
		{
			response.Redirect(url + "?x=" + qs.ToString());
		}
	}
}