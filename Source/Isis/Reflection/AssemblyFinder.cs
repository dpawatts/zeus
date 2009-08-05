using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace Isis.Reflection
{
	public class AssemblyFinder : IAssemblyFinder
	{
		private const string _assemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft|^CppCodeProvider|^VJSharpCodeProvider|^WebDev|^Castle|^Iesi|^log4net|^NHibernate|^nunit|^TestDriven|^MbUnit|^Rhino|^QuickGraph|^TestFu|^Gallio|^SoundInTheory\\.NMigration|^SoundInTheory\\.DynamicImage";
		private bool _binFolderAssembliesLoaded;

		/// <summary>The app domain to look for types in.</summary>
		public virtual AppDomain App
		{
			get { return AppDomain.CurrentDomain; }
		}

		/// <summary>Gets tne assemblies related to the current implementation.</summary>
		/// <returns>A list of assemblies that should be loaded by the N2 factory.</returns>
		public virtual IEnumerable<Assembly> GetAssemblies()
		{
			if (!_binFolderAssembliesLoaded)
			{
				_binFolderAssembliesLoaded = true;
				LoadMatchingAssemblies(HttpRuntime.BinDirectory);
			}

			List<string> addedAssemblyNames = new List<string>();
			List<Assembly> assemblies = new List<Assembly>();

			//if (LoadAppDomainAssemblies)
			AddAssembliesInAppDomain(addedAssemblyNames, assemblies);
			//AddConfiguredAssemblies(addedAssemblyNames, assemblies);

			return assemblies;
		}

		/// <summary>Iterates all assemblies in the AppDomain and if it's name matches the configured patterns add it to our list.</summary>
		/// <param name="addedAssemblyNames"></param>
		/// <param name="assemblies"></param>
		private void AddAssembliesInAppDomain(List<string> addedAssemblyNames, List<Assembly> assemblies)
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (Matches(assembly.FullName))
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
		protected virtual void LoadMatchingAssemblies(string directoryPath)
		{
			List<string> loadedAssemblyNames = new List<string>();
			foreach (Assembly a in GetAssemblies())
				loadedAssemblyNames.Add(a.FullName);

			foreach (string dllPath in Directory.GetFiles(directoryPath, "*.dll"))
			{
				try
				{
					Assembly a = Assembly.ReflectionOnlyLoadFrom(dllPath);
					if (Matches(a.FullName) && !loadedAssemblyNames.Contains(a.FullName))
						App.Load(a.FullName);
				}
				catch (BadImageFormatException ex)
				{
					Trace.TraceError(ex.ToString());
				}
			}
		}

		private static bool Matches(string assemblyFullName)
		{
			return !Matches(assemblyFullName, _assemblySkipLoadingPattern);
		}

		private static bool Matches(string assemblyFullName, string pattern)
		{
			return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
	}
}