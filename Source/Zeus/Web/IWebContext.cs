using System;

namespace Zeus.Web
{
	public interface IWebContext
	{
		/// <summary>A page instance stored in the request context.</summary>
		ContentItem CurrentPage { get; set; }

		/// <summary>The local part of the requested path, e.g. /path/to/a/page.aspx?some=query.</summary>
		Url LocalUrl { get; }

		/// <summary>Converts a virtual path to an an absolute path. E.g. ~/hello.aspx -> /MyVirtualDirectory/hello.aspx.</summary>
		/// <param name="virtualPath">The virtual url to make absolute.</param>
		/// <returns>The absolute url.</returns>
		string ToAbsolute(string virtualPath);

		/// <summary>Converts an absolute url to an app relative path. E.g. /MyVirtualDirectory/hello.aspx -> ~/hello.aspx.</summary>
		/// <param name="virtualPath">The absolute url to convert.</param>
		/// <returns>An app relative url.</returns>
		string ToAppRelative(string virtualPath);
	}
}
