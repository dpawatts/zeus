using System;
using System.Web;
using System.Web.Hosting;
using Isis.Web;
using Isis.Web.Hosting;

namespace Zeus.Web
{
	public class Global : System.Web.HttpApplication
	{
		public override void Init()
		{
			// Occurs when a security module has established the identity of the user.
			AuthenticateRequest += Global_AuthenticateRequest;

			base.Init();
		}

		/// <summary>
		/// Gets a list of default documents. Override if you need to change which documents are actually tried.
		/// </summary>
		/// <param name="uri">The URL of the request that is determined to need a default document</param>
		/// <returns>null or a list of default documents to try</returns>
		protected virtual string[] GetDefaultDocuments(Uri uri)
		{
			return new[] { "default.aspx" };
		}

		/// <summary>
		/// Code to emulate IIS handling of folders / default documents.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>
		/// Verify if this code is really needed when running under IIS7 - it could most likely be disabled then...
		/// </remarks>
		private void Global_AuthenticateRequest(object sender, EventArgs e)
		{
			// Emulate IIS directory redirection and default document handling.
			string path = Request.Url.AbsolutePath;

			// If we're referring to a directory but no document, try to find an existing default document.
			if (path.EndsWith("/"))
			{
				// Get an array of possible choices for default documents.
				string[] defaultDocuments = GetDefaultDocuments(Request.Url);
				if (defaultDocuments != null)
				{
					foreach (string defaultDocument in defaultDocuments)
					{
						Url defaultUrl = new Url(Request.Url);
						defaultUrl = defaultUrl.AppendSegment(defaultDocument);
						// If this path actually exists...
						if (HostingEnvironment.VirtualPathProvider.FileExists(defaultUrl.Path))
						{
							// ....rewrite the path to refer to this default document, and serve it up
							HttpContext.Current.RewritePath(defaultUrl.Path + defaultUrl.Query);
							break;
						}
					}
				}
				return;
			}

			// Request URL does not end with a slash, check if we have a corresponding directory
			if (HostingEnvironment.VirtualPathProvider.DirectoryExists(path))
			{
				// TODO - check if this may be a problem - redirecting with host name?
				// if we're referring to an existing directory, add a "/" and redirect
				path += "/";

				// NOTE! Since we are changing the nesting level of the request, we must do a redirect to ensure
				// that the browser requesting the information is in sync with regards to the base URL.
				Response.Redirect(path);
			}
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			OnApplicationStart(e);
		}

		protected virtual void OnApplicationStart(EventArgs e)
		{
			// TODO - add this as a plugin
			HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourcePathProvider());			
		}
	}
}