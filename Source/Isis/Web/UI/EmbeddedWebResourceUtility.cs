using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Isis.Web.Configuration;
using Isis.Web.Hosting;

namespace Isis.Web.UI
{
	public static class EmbeddedWebResourceUtility
	{
		#region Fields

		/// <summary>
		/// The name of the configuration section containing the list of assemblies
		/// that should participate in the virtual filesystem.
		/// </summary>
		public const string ConfigSectionName = "isis.web/embeddedResources";

		private static readonly EmbeddedWebResourceFileCollection _files = new EmbeddedWebResourceFileCollection();

		#endregion

		#region Constructor

		static EmbeddedWebResourceUtility()
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
		public static EmbeddedWebResourceFileCollection Files
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
			return (retVal != null) ? retVal.WebResourceAssemblies : new EmbeddedResourceAssemblyCollection();
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
				throw new ArgumentNullException("assemblyName");
			if (assemblyName.Length == 0)
				throw new ArgumentOutOfRangeException("assemblyName");

			Assembly assembly = Assembly.Load(assemblyName);

			// Get the embedded files specified in the assembly; bail early if there aren't any.
			EmbeddedWebResourceAttribute[] attribs = (EmbeddedWebResourceAttribute[]) assembly.GetCustomAttributes(typeof(EmbeddedWebResourceAttribute), true);
			if (attribs.Length == 0)
				return;

			// Get the complete set of embedded resource names in the assembly; bail early if there aren't any.
			List<string> assemblyResourceNames = new List<string>(assembly.GetManifestResourceNames());
			if (assemblyResourceNames.Count == 0)
				return;

			foreach (EmbeddedWebResourceAttribute attrib in attribs)
			{
				// Ensure the resource specified actually exists in the assembly
				if (!assemblyResourceNames.Contains(attrib.ResourcePath))
					continue;

				// Map the path into the web application
				string mappedPath;
				try
				{
					mappedPath = VirtualPathUtility.ToAbsolute(EmbeddedResourceUtility.MapResourceToWebApplication(assemblyPath, attrib.ResourceNamespace, attrib.ResourcePath));
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
				EmbeddedWebResourceFile file = new EmbeddedWebResourceFile(mappedPath, assembly, attrib.ResourcePath, attrib.ContentType);
				if (Files.Contains(file.VirtualPath))
					continue;

				// The file is unique; add it to the filesystem
				Files.Add(file);
			}
		}

		public static string GetUrl(Assembly assembly, string resourcePath)
		{
			// Get base namespace from original EmbeddedResourceFileAttribute registrations.
			EmbeddedWebResourceFile virtualFile = Files.SingleOrDefault(f => f.ContainingAssembly == assembly && f.ResourcePath == resourcePath);
			if (virtualFile == null)
				throw new Exception(string.Format("Could not find virtual file matching assembly '{0}' and resourcePath '{1}'", assembly.FullName, resourcePath));
			return virtualFile.VirtualPath;
		}

		#endregion
	}
}