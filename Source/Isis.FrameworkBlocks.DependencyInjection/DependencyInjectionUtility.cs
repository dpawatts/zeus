using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject;

namespace Isis.FrameworkBlocks.DependencyInjection
{
	public static class DependencyInjectionUtility
	{
		/// <summary>
		/// Registers all components of the specified type in the specified assembly.
		/// </summary>
		/// <param name="kernel"></param>
		/// <param name="assembly">The assembly to search for controllers.</param>
		/// <param name="namingConvention">The naming convention to use for the controllers.</param>
		public static void RegisterAllComponentsTransient<TService>(IKernel kernel, Assembly assembly, Func<Type, string> namingConvention)
		{
			foreach (Type type in assembly.GetExportedTypes().Where(IsComponent<TService>))
				kernel.Bind<TService>().To(type).InTransientScope().Named(namingConvention(type));
		}

		public static void RegisterAllComponents<TService>(IKernel kernel, Assembly assembly)
		{
			foreach (Type type in assembly.GetExportedTypes().Where(IsComponent<TService>))
				kernel.Bind<TService>().To(type);
		}

		public static void RegisterAllComponents<TService>(IKernel kernel, IEnumerable<string> filenames)
		{
			foreach (Assembly assembly in FindAssembliesWithComponents<TService>(filenames).Select(name => Assembly.Load(name)))
				foreach (Type type in assembly.GetExportedTypes().Where(IsComponent<TService>))
					kernel.Bind<TService>().To(type);
		}

		private static bool IsComponent<TService>(Type type)
		{
			return typeof(TService).IsAssignableFrom(type) && type.IsPublic && !type.IsAbstract && !type.IsInterface;
		}

		private static IEnumerable<AssemblyName> FindAssembliesWithComponents<TService>(IEnumerable<string> filenames)
		{
			AppDomain temporaryDomain = CreateTemporaryAppDomain();

			foreach (string file in filenames)
			{
				Assembly assembly;

				try
				{
					var name = new AssemblyName { CodeBase = file };
					assembly = temporaryDomain.Load(name);
				}
				catch (BadImageFormatException)
				{
					// Ignore native assemblies
					continue;
				}

				if (assembly.HasComponents<TService>())
					yield return assembly.GetName();
			}

			AppDomain.Unload(temporaryDomain);
		}

		private static AppDomain CreateTemporaryAppDomain()
		{
			return AppDomain.CreateDomain(
				"IsisComponentLoader",
				AppDomain.CurrentDomain.Evidence,
				AppDomain.CurrentDomain.SetupInformation);
		}

		public static bool HasComponents<TService>(this Assembly assembly)
		{
			return assembly.GetExportedTypes().Any(IsLoadableComponent<TService>);
		}

		private static bool IsLoadableComponent<TService>(Type type)
		{
			return typeof(TService).IsAssignableFrom(type)
				&& !type.IsAbstract
				&& !type.IsInterface
				&& type.GetConstructor(Type.EmptyTypes) != null;
		}
	}
}