using System.Collections;
using System.Security.Principal;
using System.Web;

namespace Isis.Web
{
	public interface IWebContext
	{
		#region Properties

		/// <summary>The handler associated with this request.</summary>
		IHttpHandler Handler { get; }

		/// <summary>Gets wether there is a web context availabe.</summary>
		bool IsWeb { get; }

		/// <summary>The local part of the requested path, e.g. /path/to/a/page.aspx?some=query.</summary>
		Url LocalUrl { get; }

		/// <summary>The current request object.</summary>
		HttpRequestBase Request { get; }

		/// <summary>The current response object.</summary>
		HttpResponseBase Response { get; }

		/// <summary>Gets a dictionary of request scoped items.</summary>
		IDictionary RequestItems { get; }

		/// <summary>Gets the current user principal (may be null).</summary>
		IPrincipal User { get; }

		/// <summary>The local part of the requested path, e.g. http://zeuscms.com/path/to/a/page.aspx?some=query.</summary>
		Url Url { get; }

		#endregion

		#region Methods

		string MapPath(string path);

		/// <summary>Rewrites the request to the given path.</summary>
		/// <param name="path">The path to the template that will handle the request.</param>
		void RewritePath(string path);

		/// <summary>Converts a virtual path to an an absolute path. E.g. ~/hello.aspx -> /MyVirtualDirectory/hello.aspx.</summary>
		/// <param name="virtualPath">The virtual url to make absolute.</param>
		/// <returns>The absolute url.</returns>
		string ToAbsolute(string virtualPath);

		/// <summary>Converts an absolute url to an app relative path. E.g. /MyVirtualDirectory/hello.aspx -> ~/hello.aspx.</summary>
		/// <param name="virtualPath">The absolute url to convert.</param>
		/// <returns>An app relative url.</returns>
		string ToAppRelative(string virtualPath);

		#endregion
	}
}