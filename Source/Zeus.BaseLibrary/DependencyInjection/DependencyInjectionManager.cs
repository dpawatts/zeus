using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Ninject;
using Ninject.Planning.Bindings;

namespace Zeus.BaseLibrary.DependencyInjection
{
	public class DependencyInjectionManager
	{
		private readonly InitializableKernel _kernel;

		private class InitializableKernel : StandardKernel
		{
			private readonly List<IBinding> _bindings = new List<IBinding>();

			public InitializableKernel()
			{
				
			}

			public override void AddBinding(IBinding binding)
			{
				_bindings.Add(binding);
				base.AddBinding(binding);
			}

			public void InitializeServices()
			{
				Type initializableInterfaceType = typeof(IInitializable);
				Type startableInterfaceType = typeof(IStartable);
				foreach (IBinding binding in _bindings)
					if (initializableInterfaceType.IsAssignableFrom(binding.Service) || startableInterfaceType.IsAssignableFrom(binding.Service))
						this.Get(binding.Service); // Force creation.
			}
		}

		public DependencyInjectionManager()
		{
			// Create kernel.
			_kernel = new InitializableKernel();
			try
			{
				// Get all DLLS in bin folder.
				IEnumerable<string> files = Directory.GetFiles(GetBinDirectory(), "*.dll");
                //IEnumerable<string> files = Directory.GetFiles(Path.GetDirectoryName(GetType().Assembly.Location), "*.dll");
                
                // Load modules in Zeus DLLs first.
                _kernel.Load(FindAssemblies(files.Where(s => Path.GetFileName(s).StartsWith("Zeus."))));

				// Then load non-Zeus DLLs - this gives projects a chance to override Zeus modules.
				// Actually we just load all DLLs, because DLLs that have already been loaded
				// won't get loaded again.
				_kernel.Load(FindAssemblies(files.Where(s => !Path.GetFileName(s).StartsWith("Zeus."))));
			}
			catch (TypeLoadException ex)
			{
				
			}
		}

        /// <summary>
        /// Gets the direcory containing the application
        /// </summary>
        /// <returns></returns>
        private string GetBinDirectory()
        {
            try
            {
                return Path.GetDirectoryName(HttpRuntime.BinDirectory);
            }
            catch (System.Exception)
            {
                return Path.GetDirectoryName(GetType().Assembly.Location);
            }
        }

        private static IEnumerable<Assembly> FindAssemblies(IEnumerable<string> filenames)
		{
			return FindAssemblyNames(filenames, a => true).Select(an => Assembly.Load(an));
		}

		private static IEnumerable<AssemblyName> FindAssemblyNames(IEnumerable<string> filenames, Predicate<Assembly> filter)
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
			_kernel.InitializeServices();
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