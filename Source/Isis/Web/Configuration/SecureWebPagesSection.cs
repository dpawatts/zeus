using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Isis.Web.Configuration
{
	/// <summary>
	/// The possible ways to ignore HTTP handlers.
	/// </summary>
	public enum SecureWebPageIgnoreHandlers
	{
		/// <summary>
		/// Indicates that the module should ignore the built-in HTTP handlers.
		/// <list type="bullet">
		///		<item>Trace.axd</item>
		///		<item>WebResource.axd</item>
		/// </list>
		/// </summary>
		BuiltIn,

		/// <summary>
		/// Indicates that the module should ignore all files with extensions corresponding
		/// to the standard for HTTP handlers. Currently, that is .axd files.
		/// </summary>
		WithStandardExtensions,

		/// <summary>
		/// Indicates that the module will not ignore handlers unless specifically 
		/// specified in the files or directories entries.
		/// </summary>
		/// <remarks>
		/// This was the default behavior prior to version 3.1.
		/// </remarks>
		None
	}

	/// <summary>
	/// The different modes supported for the &lt;secureWebPages&gt; configuration section.
	/// </summary>
	public enum SecureWebPageMode
	{
		/// <summary>
		/// Indicates that web page security is on and all requests should be monitored.
		/// </summary>
		On,

		/// <summary>
		/// Only remote requests are to be monitored.
		/// </summary>
		RemoteOnly,

		/// <summary>
		/// Only local requests are to be monitored.
		/// </summary>
		LocalOnly,

		/// <summary>
		/// Web page security is off and no monitoring should occur.
		/// </summary>
		Off
	}

	/// <summary>
	/// The different modes for bypassing security warnings.
	/// </summary>
	public enum SecurityWarningBypassMode
	{
		/// <summary>
		/// Always bypass security warnings when switching to an unencrypted page.
		/// </summary>
		AlwaysBypass,

		/// <summary>
		/// Only bypass security warnings when switching to an unencrypted page if the proper query parameter is present.
		/// </summary>
		BypassWithQueryParam,

		/// <summary>
		/// Never bypass security warnings when switching to an unencrypted page.
		/// </summary>
		NeverBypass
	}

	/// <summary>
	/// Contains the settings of a secureWebPages configuration section.
	/// </summary>
	public class SecureWebPagesSection : ConfigurationSection
	{

		/// <summary>
		/// Creates an instance of SecureWebPageSettings.
		/// </summary>
		public SecureWebPagesSection()
			: base()
		{
		}

		#region Properties

		/// <summary>
		/// Gets or sets the name of the query parameter that will indicate to the module to bypass
		/// any security warning if WarningBypassMode = BypassWithQueryParam.
		/// </summary>
		[ConfigurationProperty("bypassQueryParamName")]
		public string BypassQueryParamName
		{
			get { return (string) this["bypassQueryParamName"]; }
			set { this["bypassQueryParamName"] = value; }
		}

		/// <summary>
		/// Gets or sets the path to a URI for encrypted redirections, if any.
		/// </summary>
		[ConfigurationProperty("encryptedUri"), RegexStringValidator(@"^(?:|(?:https://)?[\w\-][\w\.\-,]*(?:\:\d+)?(?:/[\w\.\-]+)*/?)$")]
		public string EncryptedUri
		{
			get { return (string) this["encryptedUri"]; }
			set
			{
				if (!string.IsNullOrEmpty(value))
					this["encryptedUri"] = value;
				else
					this["encryptedUri"] = null;
			}
		}

		/// <summary>
		/// Gets or sets a flag indicating how to ignore HTTP handlers, if at all.
		/// </summary>
		[ConfigurationProperty("ignoreHandlers", DefaultValue = SecureWebPageIgnoreHandlers.BuiltIn)]
		public SecureWebPageIgnoreHandlers IgnoreHandlers
		{
			get { return (SecureWebPageIgnoreHandlers) this["ignoreHandlers"]; }
			set { this["ignoreHandlers"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether or not to maintain the current path when redirecting
		/// to a different host.
		/// </summary>
		[ConfigurationProperty("maintainPath", DefaultValue = true)]
		public bool MaintainPath
		{
			get { return (bool) this["maintainPath"]; }
			set { this["maintainPath"] = value; }
		}

		/// <summary>
		/// Gets or sets the mode indicating how the secure web page settings handled.
		/// </summary>
		[ConfigurationProperty("mode", DefaultValue = SecureWebPageMode.On)]
		public SecureWebPageMode Mode
		{
			get { return (SecureWebPageMode) this["mode"]; }
			set { this["mode"] = value; }
		}

		/// <summary>
		/// Gets the collection of directory settings read from the configuration section.
		/// </summary>
		[ConfigurationProperty("directories")]
		public SecureWebPageDirectorySettingCollection Directories
		{
			get { return (SecureWebPageDirectorySettingCollection) this["directories"]; }
		}

		/// <summary>
		/// Gets the collection of file settings read from the configuration section.
		/// </summary>
		[ConfigurationProperty("files")]
		public SecureWebPageFileSettingCollection Files
		{
			get { return (SecureWebPageFileSettingCollection) this["files"]; }
		}

		/// <summary>
		/// Gets the collection of patterns read from the configuration section.
		/// </summary>
		[ConfigurationProperty("patterns")]
		public SecureWebPagePatternSettingCollection Patterns
		{
			get { return (SecureWebPagePatternSettingCollection) this["patterns"]; }
		}

		/// <summary>
		/// Gets or sets the path to a URI for unencrypted redirections, if any.
		/// </summary>
		[ConfigurationProperty("unencryptedUri"), RegexStringValidator(@"^(?:|(?:http://)?[\w\-][\w\.\-,]*(?:\:\d+)?(?:/[\w\.\-]+)*/?)$")]
		public string UnencryptedUri
		{
			get { return (string) this["unencryptedUri"]; }
			set
			{
				if (!string.IsNullOrEmpty(value))
					this["unencryptedUri"] = value;
				else
					this["unencryptedUri"] = null;
			}
		}

		/// <summary>
		/// Gets or sets the bypass mode indicating whether or not to bypass security warnings
		/// when switching to a unencrypted page.
		/// </summary>
		[ConfigurationProperty("warningBypassMode", DefaultValue = SecurityWarningBypassMode.BypassWithQueryParam)]
		public SecurityWarningBypassMode WarningBypassMode
		{
			get { return (SecurityWarningBypassMode) this["warningBypassMode"]; }
			set { this["warningBypassMode"] = value; }
		}

		#endregion
	}
}
