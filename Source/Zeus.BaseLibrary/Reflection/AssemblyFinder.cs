using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;

namespace Zeus.BaseLibrary.Reflection
{
	/// <summary>
	/// Loads assemblies from disk and caches them
	/// </summary>
	/// <remarks>Purloined shamelessly from https://github.com/umbraco/Umbraco-CMS/blob/v8/dev/src/Umbraco.Core/Composing/TypeFinder.cs</remarks>
	public class AssemblyFinder : IAssemblyFinder
	{
		private static volatile HashSet<Assembly> _localFilteredAssemblyCache;
		private static readonly object _localFilteredAssemblyCacheLocker = new object();

		private static string _binDirectoryPath = null;

		public static bool IsHosted => HttpContext.Current != null || HostingEnvironment.IsHosted;

		/// <summary>Gets tne assemblies related to the current implementation.</summary>
		/// <returns>A list of assemblies that should be loaded by the N2 factory.</returns>
		public IEnumerable<Assembly> GetAssemblies()
		{
			return GetAssembliesWithKnownExclusions(null);

			//if (!_binFolderAssembliesLoaded)
			//{
			//	_binFolderAssembliesLoaded = true;
			//	if (IsHosted)
			//	{
			//		GetFilteredAssemblies()
			//	}
			//}

			//List<string> addedAssemblyNames = new List<string>();
			//List<Assembly> assemblies = new List<Assembly>();

			//AddAssembliesInAppDomain(ref addedAssemblyNames, ref assemblies);

			//return assemblies;
		}

        /// <summary>
        /// Gets the directory containing the application
        /// </summary>
        /// <returns></returns>
        private static string GetBinDirectory()
        {
			if (string.IsNullOrEmpty(_binDirectoryPath))
			{
				try
				{
					_binDirectoryPath = Path.GetDirectoryName(HttpRuntime.BinDirectory);
				}
				catch (Exception)
				{
					_binDirectoryPath = GetAssemblyFile(Assembly.GetExecutingAssembly()).Directory.FullName;
				}
			}
			return _binDirectoryPath;
		}

        /// <summary>Iterates all assemblies in the AppDomain and if it's name matches the configured patterns add it to our list.</summary>
        /// <param name="addedAssemblyNames"></param>
        /// <param name="assemblies"></param>
        private void AddAssembliesInAppDomain(ref List<string> addedAssemblyNames, ref List<Assembly> assemblies)
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (!Matches(assembly.FullName))
				{
					if (!addedAssemblyNames.Contains(assembly.FullName))
					{
						assemblies.Add(assembly);
						addedAssemblyNames.Add(assembly.FullName);
					}
				}
			}
		}

		/// <summary>Makes sure matching assemblies in the supplied folder are loaded in the app domain.</summary>
		/// <param name="directoryPath">The physical path to a directory containing dlls to load in the app domain.</param>
		//protected virtual void LoadMatchingAssemblies(string directoryPath)
		//{
		//	List<string> loadedAssemblyNames = new List<string>();
		//	foreach (Assembly a in GetAssemblies())
		//		loadedAssemblyNames.Add(a.FullName);

		//	foreach (string dllPath in Directory.GetFiles(directoryPath, "*.dll"))
		//	{
		//		try
		//		{
		//			Assembly a = Assembly.ReflectionOnlyLoadFrom(dllPath);
		//			if (!Matches(a.FullName) && !loadedAssemblyNames.Contains(a.FullName))
		//				App.Load(a.FullName);
		//		}
		//		catch (BadImageFormatException ex)
		//		{
		//			Trace.TraceError(ex.ToString());
		//		}
		//	}
		//}

		/// <summary>
		/// lazily load a reference to all assemblies and only local assemblies.
		/// </summary>
		/// <remarks>
		/// We do this because we cannot use AppDomain.Current.GetAssemblies() as this will return only assemblies that have been
		/// loaded in the CLR, not all assemblies.
		/// See these threads:
		/// http://stackoverflow.com/questions/3552223/asp-net-appdomain-currentdomain-getassemblies-assemblies-missing-after-app
		/// http://stackoverflow.com/questions/2477787/difference-between-appdomain-getassemblies-and-buildmanager-getreferencedassembl
		/// </remarks>
		internal static HashSet<Assembly> GetAllAssemblies()
		{
			return AllAssemblies.Value;
		}

		private static readonly Lazy<HashSet<Assembly>> AllAssemblies = new Lazy<HashSet<Assembly>>(() =>
		{
			HashSet<Assembly> assemblies = null;

			try
			{
				try
				{
					assemblies = new HashSet<Assembly>
					(
						BuildManager
						.GetReferencedAssemblies()
						.Cast<Assembly>()
					);
				}
				catch (InvalidOperationException e)
				{
					if (!(e.InnerException is SecurityException))
					{
						throw;
					}
				}

				if (assemblies == null)
				{
					var binDirectory = GetBinDirectory();
					var files = Directory.EnumerateFiles(binDirectory, "*.dll", SearchOption.TopDirectoryOnly);
					assemblies = new HashSet<Assembly>();
					foreach (var dll in files)
					{
						try
						{
							var name = AssemblyName.GetAssemblyName(dll);
							var assembly = Assembly.Load(name);
							assemblies.Add(assembly);
						}
						catch (Exception e)
						{
							if (e is SecurityException || e is BadImageFormatException)
							{
								//swallow these exceptions
							}
							else
							{
								throw;
							}
						}
					}
				}

				//if for some reason they are still no assemblies, then use the AppDomain to load in already loaded assemblies.
				if (assemblies == null || assemblies.Count <= 0)
				{
					foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
					{
						assemblies.Add(a);
					}
				}
			}
			catch (InvalidOperationException e)
			{
				if (!(e.InnerException is SecurityException))
				{
					throw;
				}
			}
			return assemblies;
		});

		/// <summary>
		/// Return a distinct list of found local Assemblies and excluding the ones passed in and excluding the exclusion list filter
		/// </summary>
		/// <param name="excludeFromResults"></param>
		/// <param name="exclusionFilter"></param>
		/// <returns></returns>
		private static IEnumerable<Assembly> GetFilteredAssemblies(
			IEnumerable<Assembly> excludeFromResults = null,
			string[] exclusionFilter = null)
		{
			if (excludeFromResults == null)
			{
				excludeFromResults = new HashSet<Assembly>();
			}

			if (exclusionFilter == null)
			{
				exclusionFilter = new string[] { };
			}

			return GetAllAssemblies()
				.Where(x => !excludeFromResults.Contains(x)
							&& !x.GlobalAssemblyCache
							&& !Matches(x.FullName));
		}

		/// <summary>
		/// Return a list of found local Assemblies excluding the known assemblies we don't want to scan
		/// and excluding the ones passed in and excluding the exclusion list filter, the results of this are
		/// cached for performance reasons.
		/// </summary>
		/// <param name="excludeFromResults"></param>
		/// <returns></returns>
		internal static HashSet<Assembly> GetAssembliesWithKnownExclusions(
			IEnumerable<Assembly> excludeFromResults = null)
		{
			lock (_localFilteredAssemblyCacheLocker)
			{
				if (_localFilteredAssemblyCache != null)
					return _localFilteredAssemblyCache;

				var assemblies = GetFilteredAssemblies(excludeFromResults, KnownAssemblyExclusionFilter);
				_localFilteredAssemblyCache = new HashSet<Assembly>(assemblies);
				return _localFilteredAssemblyCache;
			}
		}

		/// <summary>
		/// Checks our exclusion list
		/// </summary>
		/// <param name="assemblyFullName"></param>
		/// <returns></returns>
		/// <remarks>Uses an indexof with StringComparison.OrdinalIgnoreCase to speed up check compared to regex</remarks>
		private static bool Matches(string assemblyFullName)
		{
			return KnownAssemblyExclusionFilter.Any(x => assemblyFullName.StartsWith(x, StringComparison.OrdinalIgnoreCase));
		}

		/// <summary>
		/// this is our assembly filter to filter out known types that def don't contain types we'd like to find or plugins
		/// </summary>
		/// <remarks>
		/// NOTE the comma vs period... comma delimits the name in an Assembly FullName property so if it ends with comma then its an exact name match
		/// NOTE this means that "foo." will NOT exclude "foo.dll" but only "foo.*.dll"
		/// </remarks>
		internal static readonly string[] KnownAssemblyExclusionFilter = {
			"Antlr3.",
			"AutoMapper,",
			"AutoMapper.",
			"Autofac,", // DI
            "Autofac.",
			"AzureDirectory,",
			"Castle.", // DI, tests
            "ClientDependency.",
			"CookComputing.",
			"CSharpTest.", // BTree for NuCache
            "DataAnnotationsExtensions,",
			"DataAnnotationsExtensions.",
			"Dynamic,",
			"Examine,",
			"Examine.",
			"HtmlAgilityPack,",
			"HtmlAgilityPack.",
			"HtmlDiff,",
			"ICSharpCode.",
			"Iesi.Collections,", // used by NHibernate
            "LightInject.", // DI
            "LightInject,",
			"Lucene.",
			"Markdown,",
			"Microsoft.",
			"MiniProfiler,",
			"Moq,",
			"MySql.",
			"NHibernate,",
			"NHibernate.",
			"Newtonsoft.",
			"NPoco,",
			"NuGet.",
			"RouteDebugger,",
			"Semver.",
			"Serilog.",
			"Serilog,",
			"ServiceStack.",
			"SqlCE4Umbraco,",
			"Superpower,", // used by Serilog
			"SoundInTheory.DynamicImage,",
			"SoundInTheory.NMigration,",
            "System.",
			"TidyNet,",
			"TidyNet.",
			"WebDriver,",
			"itextsharp,",
			"mscorlib,",
			"nunit.framework,",
		};

		public static FileInfo GetAssemblyFile(Assembly assemblyName)
		{
			var codeBase = assemblyName.CodeBase;
			var uri = new Uri(codeBase);
			var path = uri.LocalPath;
			return new FileInfo(path);
		}
	}
}