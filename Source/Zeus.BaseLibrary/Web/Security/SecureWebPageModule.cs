using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Isis.Web.Configuration;
using System.Web;
using System.Web.Configuration;

namespace Isis.Web.Security
{
	/// <summary>
	/// Taken from http://www.codeproject.com/KB/web-security/WebPageSecurity_v2.aspx.
	/// 
	/// Hooks the application's BeginRequest event in order to request the current 
	/// page securely if specified in the configuration file.
	/// </summary>
	public class SecureWebPageModule : IHttpModule
	{

		/// <summary>
		/// Initializes an instance of this class.
		/// </summary>
		public SecureWebPageModule()
		{
		}

		/// <summary>
		/// Disposes of any resources used.
		/// </summary>
		public void Dispose()
		{
			// No resources were used.
		}

		/// <summary>
		/// Occurs just before the SecureWebPageModule evaluates the current request.
		/// </summary>
		public event BeforeEvaluateRequestEventHandler BeforeEvaluateRequest;

		/// <summary>
		/// Initializes the module by hooking the application's BeginRequest event if indicated by the config settings.
		/// </summary>
		/// <param name="context">The HttpApplication this module is bound to.</param>
		public void Init(HttpApplication context)
		{
			if (context != null)
			{
				// Get the settings for the secureWebPages section.
				SecureWebPagesSection Settings = WebConfigurationManager.GetSection("isis.web/secureWebPages") as SecureWebPagesSection;
				if (Settings != null && Settings.Mode != SecureWebPageMode.Off)
				{
					// Store the settings in application state for quick access on each request.
					context.Application["SecureWebPageSettings"] = Settings;

					// Add a reference to the Application_ProcessRequest handler for the application's
					// AcquireRequestState event.
					// * This ensures that the session ID is available for cookie-less session processing.
					context.AcquireRequestState += new EventHandler(this.Application_ProcessRequest);
				}
			}
		}

		/// <summary>
		/// Raises the BeforeEvaluateRequest event.
		/// </summary>
		/// <param name="e">The EvaluateRequestEventArgs used for the event.</param>
		protected void OnBeforeEvaluateRequest(EvaluateRequestEventArgs e)
		{
			// Raise the event.
			BeforeEvaluateRequestEventHandler Handler = BeforeEvaluateRequest;
			if (Handler != null)
				Handler(this, e);
		}

		/// <summary>
		/// Process this request by evaluating it appropriately.
		/// </summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="e">EventArgs passed in.</param>
		private void Application_ProcessRequest(Object source, EventArgs e)
		{
			// Cast the source as an HttpApplication instance.
			HttpApplication Context = source as HttpApplication;
			if (Context != null)
			{
				// Retrieve the settings from application state.
				SecureWebPagesSection Settings = (SecureWebPagesSection) Context.Application["SecureWebPageSettings"];

				// Call the BeforeEvaluateRequest event and check if a subscriber indicated to cancel the 
				// evaluation of the current request.
				EvaluateRequestEventArgs Args = new EvaluateRequestEventArgs(Context, Settings);
				OnBeforeEvaluateRequest(Args);

				if (!Args.CancelEvaluation)
				{
					// Evaluate the response against the settings.
					SecurityType Secure = RequestEvaluator.Evaluate(Context.Request, Settings, false);

					// Take appropriate action.
					if (Secure == SecurityType.Secure)
						SslHelper.RequestSecurePage(Settings);
					else if (Secure == SecurityType.Insecure)
						SslHelper.RequestUnsecurePage(Settings);
				}
			}
		}
	}

	/// <summary>
	/// Represents the method that handles the event raised just before a request is evaluated by 
	/// the SecureWebPageModule.
	/// </summary>
	/// <param name="sender">The SecureWebPageModule that is the source of the event.</param>
	/// <param name="e">An EvaluateRequestEventArgs that contains the event data.</param>
	public delegate void BeforeEvaluateRequestEventHandler(object sender, EvaluateRequestEventArgs e);
}
