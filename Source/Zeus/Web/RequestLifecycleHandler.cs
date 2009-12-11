using System;
using System.Diagnostics;
using System.Web;
using Ninject;
using Zeus.BaseLibrary.Web;
using Zeus.Configuration;
using Zeus.Engine;
using Zeus.Installation;
using Zeus.Web.UI;

namespace Zeus.Web
{
	/// <summary>
	/// Handles the request life cycle for Zeus by invoking url rewriting, 
	/// authorizing and closing NHibernate session.
	/// </summary>
	public class RequestLifecycleHandler : IRequestLifecycleHandler, IStartable
	{
		readonly IErrorHandler errors;
		readonly IWebContext webContext;
		readonly EventBroker broker;
		readonly InstallationManager installer;
		readonly IRequestDispatcher dispatcher;

		protected bool initialized = false;
		protected bool checkInstallation = false;
		protected RewriteMethod rewriteMethod = RewriteMethod.RewriteRequest;
		protected string installerUrl = "~/admin/install/default.aspx";
		private readonly AdminSection _adminConfig;

		/// <summary>Creates a new instance of the RequestLifeCycleHandler class.</summary>
		/// <param name="webContext">The web context wrapper.</param>
		public RequestLifecycleHandler(IWebContext webContext, EventBroker broker, InstallationManager installer, IRequestDispatcher dispatcher, IErrorHandler errors, AdminSection editConfig, HostSection hostConfig)
			: this(webContext, broker, installer, dispatcher, errors)
		{
			checkInstallation = editConfig.Installer.CheckInstallationStatus;
			//installerUrl = editConfig.Installer.InstallUrl;
			rewriteMethod = hostConfig.Web.Rewrite;
			_adminConfig = editConfig;
		}

		/// <summary>Creates a new instance of the RequestLifeCycleHandler class.</summary>
		/// <param name="webContext">The web context wrapper.</param>
		public RequestLifecycleHandler(IWebContext webContext, EventBroker broker, InstallationManager installer, IRequestDispatcher dispatcher, IErrorHandler errors)
		{
			this.webContext = webContext;
			this.broker = broker;
			this.errors = errors;
			this.installer = installer;
			this.dispatcher = dispatcher;
			_adminConfig = null;
		}

		/// <summary>Subscribes to applications events.</summary>
		/// <param name="broker">The application.</param>
		public void Init(EventBroker broker)
		{
			Debug.WriteLine("RequestLifeCycleHandler.Init");

			broker.BeginRequest += Application_BeginRequest;
			broker.AuthorizeRequest += Application_AuthorizeRequest;
			broker.AcquireRequestState += Application_AcquireRequestState;
			broker.Error += Application_Error;
			broker.EndRequest += Application_EndRequest;
		}

		protected virtual void Application_BeginRequest(object sender, EventArgs e)
		{
			if (!initialized)
			{
				// we need to have reached begin request before we can do certain 
				// things in IIS7. concurrency isn't crucial here.
				initialized = true;
				if (webContext.IsWeb)
				{
					if (Url.ServerUrl == null)
						Url.ServerUrl = webContext.Url.HostUrl;
					if (checkInstallation)
						CheckInstallation();
				}
			}

			RequestAdapter controller = dispatcher.ResolveAdapter<RequestAdapter>();
			if (controller != null)
			{
				webContext.CurrentPath = controller.Path;
				controller.RewriteRequest(rewriteMethod);
			}
		}

		private void CheckInstallation()
		{
			bool isEditing = webContext.ToAppRelative(webContext.Url.Path).StartsWith("~/" + _adminConfig.Path, StringComparison.InvariantCultureIgnoreCase);
			if (!isEditing && !installer.GetStatus().IsInstalled)
			{
				webContext.Response.Redirect(installerUrl);
			}
		}

		/// <summary>Infuses the http handler (usually an aspx page) with the content page associated with the url if it implements the <see cref="IContentTemplate"/> interface.</summary>
		protected virtual void Application_AcquireRequestState(object sender, EventArgs e)
		{
			if (webContext.CurrentPath == null || webContext.CurrentPath.IsEmpty()) return;

			RequestAdapter controller = dispatcher.ResolveAdapter<RequestAdapter>();
			controller.InjectCurrentPage(webContext.Handler);
		}

		protected virtual void Application_AuthorizeRequest(object sender, EventArgs e)
		{
			if (webContext.CurrentPath == null || webContext.CurrentPath.IsEmpty()) return;

			RequestAdapter controller = dispatcher.ResolveAdapter<RequestAdapter>();
			controller.AuthorizeRequest(webContext.User);
		}

		protected virtual void Application_Error(object sender, EventArgs e)
		{
			HttpApplication application = sender as HttpApplication;
			if (application != null)
			{
				Exception ex = application.Server.GetLastError();
				if (ex != null)
				{
					errors.Notify(ex);
				}
			}
		}

		protected virtual void Application_EndRequest(object sender, EventArgs e)
		{
			webContext.Close();
		}

		#region IStartable Members

		public void Start()
		{
			broker.BeginRequest += Application_BeginRequest;
			broker.AuthorizeRequest += Application_AuthorizeRequest;
			broker.AcquireRequestState += Application_AcquireRequestState;
			broker.Error += Application_Error;
			broker.EndRequest += Application_EndRequest;
		}

		public void Stop()
		{
			broker.BeginRequest -= Application_BeginRequest;
			broker.AuthorizeRequest -= Application_AuthorizeRequest;
			broker.AcquireRequestState -= Application_AcquireRequestState;
			broker.Error -= Application_Error;
			broker.EndRequest -= Application_EndRequest;
		}

		#endregion
	}
}