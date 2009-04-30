using System;
using System.Collections.Specialized;
using Isis.Web;
using Zeus.Configuration;
using Zeus.Engine;

namespace Zeus.Web
{
	/// <summary>
	/// Resolves the controller to handle a certain request. Supports a default 
	/// controller or additional imprativly using the ConnectControllers method 
	/// or declarativly using the [Controls] attribute registered.
	/// </summary>
	public class RequestDispatcher : IRequestDispatcher
	{
		readonly IAspectControllerProvider aspectProvider;
		readonly IWebContext webContext;
		readonly IUrlParser parser;
		readonly IErrorHandler errorHandler;
		readonly bool rewriteEmptyExtension = true;
		readonly string[] observedExtensions = new[] { ".aspx" };


		public RequestDispatcher(IAspectControllerProvider aspectProvider, IWebContext webContext, IUrlParser parser, IErrorHandler errorHandler, HostSection config)
		{
			this.aspectProvider = aspectProvider;
			this.webContext = webContext;
			this.parser = parser;
			this.errorHandler = errorHandler;
			rewriteEmptyExtension = config.Web.ObserveEmptyExtension;
			StringCollection additionalExtensions = config.Web.ObservedExtensions;
			if (additionalExtensions != null && additionalExtensions.Count > 0)
			{
				observedExtensions = new string[additionalExtensions.Count + 1];
				additionalExtensions.CopyTo(observedExtensions, 1);
			}
			observedExtensions[0] = config.Web.Extension;
		}

		/// <summary>Resolves the controller for the current Url.</summary>
		/// <returns>A suitable controller for the given Url.</returns>
		public virtual T ResolveAspectController<T>() where T : class, IAspectController
		{
			T controller = RequestItem<T>.Instance;
			if (controller != null) return controller;

			PathData path = ResolveUrl(webContext.Url);
			controller = aspectProvider.ResolveAspectController<T>(path);

			RequestItem<T>.Instance = controller;
			return controller;
		}

		public PathData ResolveUrl(string url)
		{
			try
			{
				if (IsObservable(url)) return parser.ResolvePath(url);
			}
			catch (Exception ex)
			{
				errorHandler.Notify(ex);
			}
			return PathData.Empty;
		}

		private bool IsObservable(Url url)
		{
			if (url.LocalUrl == Url.ApplicationPath)
				return true;

			string extension = url.Extension;
			if (rewriteEmptyExtension && string.IsNullOrEmpty(extension))
				return true;
			foreach (string observed in observedExtensions)
				if (string.Equals(observed, extension, StringComparison.InvariantCultureIgnoreCase))
					return true;
			if (url.GetQuery("page") != null)
				return true;

			return false;
		}
	}
}