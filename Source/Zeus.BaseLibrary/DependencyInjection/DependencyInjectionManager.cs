using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Ninject;

namespace Zeus.BaseLibrary.DependencyInjection
{
	public class DependencyInjectionManager
	{
		private readonly IKernel _kernel;

		public DependencyInjectionManager()
		{
			// Create kernel.
			_kernel = new StandardKernel();
			try
			{
				// Get all DLLS in bin folder.
				IEnumerable<string> files = Directory.GetFiles(Path.GetDirectoryName(HttpRuntime.BinDirectory), "*.dll");

				// Load modules in Zeus DLLs first.
				_kernel.Load(FindAssemblies(files.Where(s => Path.GetFileName(s).StartsWith("Zeus."))));

				// Then load non-Zeus DLLs - this gives projects a chance to override Zeus modules.
				// Actually we just load all DLLs, because DLLs that have already been loaded
				// won't get loaded again.
				_kernel.Load(FindAssemblies(files.Where(s => !Path.GetFileName(s).StartsWith("Zeus."))));
			}
			catch (TypeLoadException)
			{
				
			}
		}

		private static IEnumerable<Assembly> FindAssemblies(IEnumerable<string> filenames)
		{
			return FindAssemblyNames(filenames, a => true).Select(an => Assembly.Load(an));
		}

		private static IEnumerable<AssemblyName> FindAssemblyNames(IEnumerable<string> filenames,
																																Predicate<Assembly> filter)
		{
			AppDomain temporaryDomain = CreateTemporaryAppDomain();

			foreach (string file in filenames)
			{
				Assembly assembly;

				if (File.Exists(file))
				{
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
				}
				else
				{
					assembly = temporaryDomain.Load(file);
				}

				if (filter(assembly))
				{
					yield return assembly.GetName();
				}
			}

			AppDomain.Unload(temporaryDomain);
		}

		private static AppDomain CreateTemporaryAppDomain()
		{
			return AppDomain.CreateDomain(
					"AssemblyScanner",
					AppDomain.CurrentDomain.Evidence,
					AppDomain.CurrentDomain.SetupInformation);
		}

		public void Initialize()
		{
			((KernelBase) _kernel).InitializeServices();
		}

		public void Bind<TService, TComponent>()
			where TComponent : TService
		{
			_kernel.Bind<TService>().To<TComponent>();
		}

		public void Bind(Type serviceType, Type componentType)
		{
			_kernel.Bind(serviceType).To(componentType);
		}

		public void Bind(Type componentType)
		{
			_kernel.Bind(componentType).ToSelf();
		}

		public void BindInstance(object instance)
		{
			if (instance == null)
				return;

			_kernel.Bind(instance.GetType()).ToConstant(instance);
		}

		public TService Get<TService>()
		{
			return _kernel.Get<TService>();
		}

		public IEnumerable<TService> GetAll<TService>()
		{
			return _kernel.GetAll<TService>();
		}

		public object Get(Type serviceType)
		{
			return _kernel.Get(serviceType);
		}
	}
}