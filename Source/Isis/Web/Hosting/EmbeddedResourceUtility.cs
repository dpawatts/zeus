using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Isis.Web.Configuration;

namespace Isis.Web.Hosting
{
	public static class EmbeddedResourceUtility
	{
		#region Fields

		/// <summary>
		/// The name of the configuration section containing the list of assemblies
		/// that should participate in the virtual filesystem.
		/// </summary>
		/// <seealso cref="EmbeddedResourcePathProvider" />
		public const string ConfigSectionName = "isis.web/embeddedResources";

		/// <summary>
		/// Internal storage for the
		/// <see cref="EmbeddedResourceUtility.Files" />
		/// property.
		/// </summary>
		/// <seealso cref="EmbeddedResourcePathProvider" />
		private static readonly EmbeddedResourceVirtualFileCollection _files = new EmbeddedResourceVirtualFileCollection();

		#endregion

		#region Constructor

		static EmbeddedResourceUtility()
		{
			EmbeddedResourceAssemblyCollection configuredAssemblyNames = GetConfiguredAssemblyNames();
			if (configuredAssemblyNames.Count != 0)
				foreach (EmbeddedResourceAssembly configuredAssembly in configuredAssemblyNames)
					ProcessEmbeddedFiles(configuredAssembly.Assembly, configuredAssembly.Path);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the collection of files served by this provider.
		/// </summary>
		/// <value>
		/// A <see cref="EmbeddedResourcePathProvider"/>
		/// that contains all of the files served by this provider.
		/// </value>
		/// <seealso cref="Isis.Web.Hosting" />
		public static EmbeddedResourceVirtualFileCollection Files
		{
			get { return _files; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the names of the configured assemblies from configuration.
		/// </summary>
		/// <returns>
		/// A <see cref="System.Collections.Specialized.StringCollection"/> with
		/// the names of the configured assemblies that should participate in this
		/// path provider.
		/// </returns>
		/// <seealso cref="EmbeddedResourcePathProvider" />
		private static EmbeddedResourceAssemblyCollection GetConfiguredAssemblyNames()
		{
			EmbeddedResourcesSection retVal = System.Configuration.ConfigurationManager.GetSection(ConfigSectionName) as EmbeddedResourcesSection;
			return (retVal != null) ? retVal.VirtualPathAssemblies : new EmbeddedResourceAssemblyCollection();
		}

		/// <summary>
		/// Reads in the embedded files from an assembly an processes them into
		/// the virtual filesystem.
		/// </summary>
		/// <param name="assemblyName">The name of the <see cref="System.Reflection.Assembly"/> to load and process.</param>
		/// <param name="assemblyPath">The root web application path (i.e. "/admin") of the resources in the specified assembly</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="assemblyName" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown if <paramref name="assemblyName" /> is <see cref="System.String.Empty" />.
		/// </exception>
		/// <exception cref="System.IO.FileNotFoundException">
		/// Thrown if the <see cref="System.Reflection.Assembly"/> indicated by
		/// <paramref name="assemblyName" /> is not found.
		/// </exception>
		/// <remarks>
		/// <para>
		/// The <paramref name="assemblyName" /> will be passed to <see cref="System.Reflection.Assembly.Load(string)"/>
		/// so the associated assembly can be processed.  If the assembly is not
		/// found, a <see cref="System.IO.FileNotFoundException"/> is thrown.
		/// </para>
		/// <para>
		/// Once the assembly is retrieved, it is queried for <see cref="Files"/>
		/// instances.  For each one found, the associated resources are processed
		/// into virtual files that will be stored in
		/// <see cref="EmbeddedResourcePathProvider"/>
		/// for later use.
		/// </para>
		/// </remarks>
		/// <seealso cref="Isis.Web.Hosting" />
		/// <seealso cref="EmbeddedResourcePathProvider" />
		private static void ProcessEmbeddedFiles(string assemblyName, string assemblyPath)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (assemblyName.Length == 0)
			{
				throw new ArgumentOutOfRangeException("assemblyName");
			}

			Assembly assembly = Assembly.Load(assemblyName);

			// Get the embedded files specified in the assembly; bail early if there aren't any.
			EmbeddedResourceFileAttribute[] attribs = (EmbeddedResourceFileAttribute[]) assembly.GetCustomAttributes(typeof(EmbeddedResourceFileAttribute), true);
			if (attribs.Length == 0)
				return;

			// Get the complete set of embedded resource names in the assembly; bail early if there aren't any.
			List<string> assemblyResourceNames = new List<string>(assembly.GetManifestResourceNames());
			if (assemblyResourceNames.Count == 0)
				return;

			foreach (EmbeddedResourceFileAttribute attrib in attribs)
			{
				// Ensure the resource specified actually exists in the assembly
				if (!assemblyResourceNames.Contains(attrib.ResourcePath))
					continue;

				// Map the path into the web application
				string mappedPath;
				try
				{
					mappedPath = VirtualPathUtility.ToAbsolute(MapResourceToWebApplication(assemblyPath, attrib.ResourceNamespace, attrib.ResourcePath));
				}
				catch (ArgumentNullException)
				{
					continue;
				}
				catch (ArgumentOutOfRangeException)
				{
					continue;
				}

				// Create the file and ensure it's unique
				EmbeddedResourceVirtualFile file = new EmbeddedResourceVirtualFile(mappedPath, assembly, attrib.ResourcePath);
				if (Files.Contains(file.VirtualPath))
					continue;

				// The file is unique; add it to the filesystem
				Files.Add(file);
			}
		}

		/// <summary>
		/// Maps an embedded resource ID into a web application relative path (~/path).
		/// </summary>
		/// <param name="assemblyPath">The root web application path (i.e. "/admin") of the resources in the specified assembly</param>
		/// <param name="baseNamespace">
		/// The base namespace of the resource to map.
		/// </param>
		/// <param name="resourcePath">
		/// The fully qualified embedded resource path to map.
		/// </param>
		/// <returns>The mapped path of the resource into the web application.</returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="baseNamespace" /> is stripped from the front of the
		/// <paramref name="resourcePath" /> and all but the last period in the remaining
		/// <paramref name="resourcePath" /> is replaced with the directory separator character
		/// ('/').  Finally, that path is mapped into a web application relative path.
		/// </para>
		/// <para>
		/// The filename being mapped must have an extension associated with it, and that
		/// extension may not have a period in it.  Only one period will be kept in the
		/// mapped filename - others will be assumed to be directory separators.  If a filename
		/// has multiple extensions (i.e., <c>My.Custom.config</c>), it will not map properly -
		/// it will end up being <c>~/My/Custom.config</c>.
		/// </para>
		/// <para>
		/// If <paramref name="baseNamespace" /> does not occur at the start of the
		/// <paramref name="resourcePath" />, an <see cref="System.InvalidOperationException"/>
		/// is thrown.
		/// </para>
		/// </remarks>
		/// <example>
		/// <para>
		/// Given a <paramref name="baseNamespace" /> of <c>MyNamespace</c>,
		/// this method will process <paramref name="resourcePath" /> as follows:
		/// </para>
		/// <list type="table">
		/// <listheader>
		/// <term><paramref name="resourcePath" /> value</term>
		/// <description>Mapping in Web App</description>
		/// </listheader>
		/// <item>
		/// <term><c>MyNamespace.Config.MyFile.config</c></term>
		/// <description><c>~/Config/MyFile.config</c></description>
		/// </item>
		/// <item>
		/// <term><c>MyNamespace.MyPage.aspx</c></term>
		/// <description><c>~/MyPage.aspx</c></description>
		/// </item>
		/// </list>
		/// </example>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="baseNamespace" /> or <paramref name="resourcePath" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown if <paramref name="baseNamespace" /> or <paramref name="resourcePath" />:
		/// <list type="bullet">
		/// <item>
		/// <description>
		/// Is <see cref="System.String.Empty"/>.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// Start or end with period.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// Contain two or more periods together (like <c>MyNamespace..MySubnamespace</c>).
		/// </description>
		/// </item>
		/// </list>
		/// </exception>
		/// <seealso cref="EmbeddedResourcePathProvider" />
		internal static string MapResourceToWebApplication(string assemblyPath, string baseNamespace, string resourcePath)
		{
			// Validate parameters
			ValidateResourcePath("assemblyPath", assemblyPath);
			ValidateResourcePath("baseNamespace", baseNamespace);
			ValidateResourcePath("resourcePath", resourcePath);

			// Ensure that the base namespace (with the period delimiter) appear in the resource path
			if (resourcePath.IndexOf(baseNamespace + ".") != 0)
				throw new InvalidOperationException("Base resource namespace must appear at the start of the embedded resource path.");

			// Remove the base namespace from the resource path
			string newResourcePath = resourcePath.Remove(0, baseNamespace.Length + 1);

			// Find the last period - that's the file extension
			int extSeparator = newResourcePath.LastIndexOf(".");

			// Replace all but the last period with a directory separator
			string resourceFilePath = newResourcePath.Substring(0, extSeparator).Replace(".", "/") + newResourcePath.Substring(extSeparator, newResourcePath.Length - extSeparator);

			// Map the path into the web app and return
			string retVal = VirtualPathUtility.Combine("~/", assemblyPath + "/" + resourceFilePath);
			return retVal;
		}

		/// <summary>
		/// Validates an embedded resource path or namespace.
		/// </summary>
		/// <param name="paramName">The name of the parameter being validated.</param>
		/// <param name="path">The path/namespace to validate.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown if <paramref name="path" />:
		/// <list type="bullet">
		/// <item>
		/// <description>
		/// Is <see cref="System.String.Empty"/>.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// Start or end with period.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// Contain two or more periods together (like <c>MyNamespace..MySubnamespace</c>).
		/// </description>
		/// </item>
		/// </list>
		/// </exception>
		/// <seealso cref="EmbeddedResourcePathProvider" />
		private static void ValidateResourcePath(string paramName, string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("paramName");
			}
			if (path.Length == 0)
			{
				throw new ArgumentOutOfRangeException("paramName");
			}
			if (path.StartsWith(".") || path.EndsWith("."))
			{
				throw new ArgumentOutOfRangeException(paramName, path, paramName + " may not start or end with a period.");
			}
			if (path.IndexOf("..") >= 0)
			{
				throw new ArgumentOutOfRangeException(paramName, path, paramName + " may not contain two or more periods together.");
			}
		}

		public static string GetUrl(Assembly assembly, string resourcePath)
		{
			// Get base namespace from original EmbeddedResourceFileAttribute registrations.
			EmbeddedResourceVirtualFile virtualFile = Files.SingleOrDefault(f => f.ContainingAssembly == assembly && f.ResourcePath == resourcePath);
			if (virtualFile == null)
				throw new Exception(string.Format("Could not find virtual file matching assembly '{0}' and resourcePath '{1}'", assembly.FullName, resourcePath));
			return virtualFile.VirtualPath;
		}

		#endregion
	}
}