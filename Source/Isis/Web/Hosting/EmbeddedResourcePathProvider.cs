using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Caching;
using System.Web.Hosting;
using Isis.Web.Configuration;

namespace Isis.Web.Hosting
{
	/// <summary>
	/// A <see cref="System.Web.Hosting.VirtualPathProvider"/> that allows serving
	/// pages from embedded resources.
	/// </summary>
	/// <remarks>
	/// <para>
	/// ASP.NET retrieves files to serve via the <see cref="System.Web.Hosting.HostingEnvironment"/>.
	/// Rather than opening a file via <see cref="System.IO.File"/>, you ask
	/// the <see cref="System.Web.Hosting.HostingEnvironment"/> for its
	/// <see cref="System.Web.Hosting.HostingEnvironment.VirtualPathProvider"/>
	/// and ask that provider for the file.  The provider will return a
	/// <see cref="System.Web.Hosting.VirtualFile"/> reference that will allow you
	/// to open a stream on the file and use the contents.
	/// </para>
	/// <para>
	/// This implementation of <see cref="System.Web.Hosting.VirtualPathProvider"/>
	/// allows you to serve files to ASP.NET through embedded resources.  Rather
	/// than deploying your web forms, user controls, etc., to the file system,
	/// you can embed the files as resources right in your assembly and deploy
	/// just your assembly.  The <see cref="System.Web.Hosting.VirtualPathProvider"/>
	/// mechanism will take care of the rest.
	/// </para>
	/// <note type="caution">
	/// Most <see cref="System.Web.Hosting.VirtualPathProvider"/> implementations
	/// handle both directories and files.  This implementation handles only files.
	/// As such, if the <see cref="System.Web.Hosting.VirtualPathProvider"/> is
	/// used to enumerate available files (as in directory browsing), files provided
	/// via embedded resource will not be included.
	/// </note>
	/// <para>
	/// To use this <see cref="System.Web.Hosting.VirtualPathProvider"/>, you need
	/// to do four things to your web application.
	/// </para>
	/// <para>
	/// First, you need to set all of the files you want to serve from your assembly
	/// as embedded resources.  By default, web forms and so forth are set as "content"
	/// files; setting them as embedded resources will package them into your assembly.
	/// </para>
	/// <para>
	/// Second, in your <c>AssemblyInfo.cs</c> file (or whichever file you are
	/// declaring your assembly attributes in) you need to add one
	/// <see cref="MapResourceToWebApplication"/> for
	/// every file you plan on serving.  This lets the provider know which embedded
	/// resources are available and which are actually resources for other purposes.
	/// Your assembly attributes will look something like this:
	/// </para>
	/// <code lang="C#">
	/// [assembly: EmbeddedResourceFileAttribute("MyNamespace.WebForm1.aspx", "MyNamespace")]
	/// [assembly: EmbeddedResourceFileAttribute("MyNamespace.UserControl1.ascx", "MyNamespace")]
	/// </code>
	/// <para>
	/// Third, you need to register this provider at application startup so ASP.NET
	/// knows to use it.  In your <c>Global.asax</c>, during <c>Application_OnStart</c>,
	/// put the following:
	/// </para>
	/// <code lang="C#">
	/// System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourcePathProvider());
	/// </code>
	/// <para>
	/// Fourth, in your <c>web.config</c> file, you need to set up a configuration
	/// section called <c>embeddedFileAssemblies</c> that lets the provider know
	/// which assemblies should be queried for embedded files.  A sample configuration
	/// section looks like this:
	/// </para>
	/// <code>
	/// &lt;configuration&gt;
	///   &lt;configSections&gt;
	///     &lt;section name="embeddedFileAssemblies" type="Paraesthesia.Configuration.StringCollectionSectionHandler, Paraesthesia.Web.Hosting.EmbeddedResourcePathProvider"/&gt;
	///   &lt;/configSections&gt;
	///   &lt;embeddedFileAssemblies&gt;
	///     &lt;add value="My.Web.Assembly"/&gt;
	///   &lt;/embeddedFileAssemblies&gt;
	///   &lt;!-- ... other web.config items ... --&gt;
	/// &lt;/configuration&gt;
	/// </code>
	/// <para>
	/// Once you have that set up, you're ready to serve files from embedded resources.
	/// Simply deploy your application without putting the embedded resource files
	/// into the filesystem.  When you visit the embedded locations, the provider
	/// will automatically retrieve the proper embedded resource.
	/// </para>
	/// <para>
	/// File paths are mapped into the application using the
	/// <see cref="EmbeddedResourcePathProvider"/>
	/// declarations and the <see cref="Isis.Web.Hosting"/>
	/// method.  This allows you to set up your web application as normal in
	/// Visual Studio and the folder structure, which automatically generates
	/// namespaces for your embedded resources, will translate into virtual folders
	/// in the embedded resource "filesystem."
	/// </para>
	/// <para>
	/// By default, files that are embedded as resources will take precedence over
	/// files in the filesystem.  If you would like the files in the filesystem
	/// to take precedence (that is, if you would like to allow the filesystem
	/// to "override" embedded files), you can set a key in the <c>appSettings</c>
	/// section of your <c>web.config</c> file that enables overrides:
	/// </para>
	/// <code>
	/// &lt;configuration&gt;
	///   &lt;!-- ... other web.config items ... --&gt;
	///   &lt;appSettings&gt;
	///     &lt;add key="Paraesthesia.Web.Hosting.EmbeddedResourcePathProvider.AllowOverrides" value="true"/&gt;
	///   &lt;/appSettings&gt;
	/// &lt;/configuration&gt;
	/// </code>
	/// <para>
	/// For more information on virtual filesystems in ASP.NET, check out
	/// <see cref="System.Web"/>.
	/// </para>
	/// </remarks>
	/// <seealso cref="System.Web" />
	/// <seealso cref="System" />
	public class EmbeddedResourcePathProvider : VirtualPathProvider
	{

		#region EmbeddedResourcePathProvider Variables

		#region Constants

		/// <summary>
		/// The standard web application "app relative root" path.
		/// </summary>
		/// <seealso cref="EmbeddedResourcePathProvider" />
		private const string ApplicationRootPath = "~/";

		#endregion


		#endregion





		#region EmbeddedResourcePathProvider Implementation

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="EmbeddedResourcePathProvider" /> class.
		/// </summary>
		/// <seealso cref="EmbeddedResourcePathProvider" />
		public EmbeddedResourcePathProvider() : base() { }

		#endregion

		#region Overrides

		/// <summary>
		/// Gets a value that indicates whether a file exists in the virtual file system.
		/// </summary>
		/// <param name="virtualPath">The path to the virtual file.</param>
		/// <returns><see langword="true" /> if the file exists in the virtual file system; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		/// <para>
		/// This override checks to see if the embedded resource file exists
		/// in memory.  If so, this method will return <see langword="true" />.
		/// If not, it returns the value from the <see cref="System.Web.Hosting.VirtualPathProvider.Previous"/>
		/// virtual path provider.
		/// </para>
		/// </remarks>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="virtualPath" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown if <paramref name="virtualPath" /> is <see cref="System.String.Empty" />.
		/// </exception>
		/// <seealso cref="EmbeddedResourcePathProvider" />
		/// <seealso cref="System.Web.Hosting.VirtualPathProvider.FileExists" />
		public override bool FileExists(string virtualPath)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			if (virtualPath.Length == 0)
			{
				throw new ArgumentOutOfRangeException("virtualPath");
			}
			string absolutePath = System.Web.VirtualPathUtility.ToAbsolute(virtualPath);
			if (EmbeddedResourceUtility.Files.Contains(absolutePath))
			{
				return true;
			}
			return base.FileExists(absolutePath);
		}

		/// <summary>
		/// Creates a cache dependency based on the specified virtual paths.
		/// </summary>
		/// <param name="virtualPath">The path to the primary virtual resource.</param>
		/// <param name="virtualPathDependencies">An array of paths to other resources required by the primary virtual resource.</param>
		/// <param name="utcStart">The UTC time at which the virtual resources were read.</param>
		/// <returns>A <see cref="System.Web.Caching.CacheDependency"/> object for the specified virtual resources.</returns>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="virtualPath" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown if <paramref name="virtualPath" /> is <see cref="System.String.Empty" />.
		/// </exception>
		/// <seealso cref="EmbeddedResourcePathProvider" />
		/// <seealso cref="System.Web.Hosting.VirtualPathProvider.GetCacheDependency" />
		public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			if (virtualPath.Length == 0)
			{
				throw new ArgumentOutOfRangeException("virtualPath");
			}
			string absolutePath = System.Web.VirtualPathUtility.ToAbsolute(virtualPath);

			// Lazy initialize the return value so we can return null if needed
			AggregateCacheDependency retVal = null;

			// Handle chained dependencies
			if (virtualPathDependencies != null)
			{
				foreach (string virtualPathDependency in virtualPathDependencies)
				{
					CacheDependency dependencyToAdd = this.GetCacheDependency(virtualPathDependency, null, utcStart);
					if (dependencyToAdd == null)
					{
						// Ignore items that have no dependency
						continue;
					}

					if (retVal == null)
					{
						retVal = new AggregateCacheDependency();
					}
					retVal.Add(dependencyToAdd);
				}
			}

			// Handle the primary file
			CacheDependency primaryDependency = null;
			if (FileHandledByBaseProvider(absolutePath))
				primaryDependency = base.GetCacheDependency(absolutePath, null, utcStart);
			else
				primaryDependency = new CacheDependency(((EmbeddedResourceVirtualFile) EmbeddedResourceUtility.Files[absolutePath]).ContainingAssembly.Location, utcStart);

			if (primaryDependency != null)
			{
				if (retVal == null)
					retVal = new AggregateCacheDependency();
				retVal.Add(primaryDependency);
			}

			return retVal;
		}

		/// <summary>
		/// Gets a virtual file from the virtual file system.
		/// </summary>
		/// <param name="virtualPath">The path to the virtual file.</param>
		/// <returns>A descendent of the <see cref="System.Web.Hosting.VirtualFile"/> class that represents a file in the virtual file system.</returns>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="virtualPath" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown if <paramref name="virtualPath" /> is <see cref="System.String.Empty" />.
		/// </exception>
		/// <seealso cref="EmbeddedResourcePathProvider" />
		/// <seealso cref="System.Web.Hosting.VirtualPathProvider.GetFile" />
		public override VirtualFile GetFile(string virtualPath)
		{
			// virtualPath comes in absolute form: /MyApplication/Subfolder/OtherFolder/Control.ascx
			// * ToAppRelative: ~/Subfolder/OtherFolder/Control.ascx
			// * ToAbsolute: /MyApplication/Subfolder/OtherFolder/Control.ascx
			if (virtualPath == null)
				throw new ArgumentNullException("virtualPath");
			if (virtualPath.Length == 0)
				throw new ArgumentOutOfRangeException("virtualPath");

			string absolutePath = System.Web.VirtualPathUtility.ToAbsolute(virtualPath);
			if (FileHandledByBaseProvider(absolutePath))
				return base.GetFile(absolutePath);
			return (VirtualFile) EmbeddedResourceUtility.Files[absolutePath];
		}

		#endregion

		#region Methods

		#region Instance

		/// <summary>
		/// Determines if a file should be handled by the base provider or if
		/// it should be handled by this provider.
		/// </summary>
		/// <param name="absolutePath">The absolute path to the file to check.</param>
		/// <returns>
		/// <see langword="true" /> if processing of the file at
		/// <paramref name="absolutePath" /> should be done by the base provider;
		/// <see langword="false" /> if this provider should handle it.
		/// </returns>
		/// <seealso cref="EmbeddedResourcePathProvider" />
		private static bool FileHandledByBaseProvider(string absolutePath)
		{
			return !EmbeddedResourceUtility.Files.Contains(absolutePath);
		}

		#endregion

		#endregion

		#endregion
	}
}