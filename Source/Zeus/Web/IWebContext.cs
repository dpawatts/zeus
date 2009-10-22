using System.Web;

namespace Zeus.Web
{
	public interface IWebContext : BaseLibrary.Web.IWebContext
	{
		/// <summary>A page instance stored in the request context.</summary>
		ContentItem CurrentPage { get; set; }

		/// <summary>The template used to serve this request.</summary>
		PathData CurrentPath { get; set; }

		HttpSessionStateBase Session { get; }

		/// <summary>The physical path on disk to the requested page.</summary>
		string PhysicalPath { get; }

		/// <summary>Closes any endable resources at the end of the request.</summary>
		void Close();

		/// <summary>Transferes the request to the given path.</summary>
		/// <param name="path">The path to the template that will handle the request.</param>
		void TransferRequest(string path);

		string GetFullyQualifiedUrl(string url);
	}
}
