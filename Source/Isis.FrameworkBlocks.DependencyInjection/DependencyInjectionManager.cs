using System;
using Ninject;

namespace Isis.FrameworkBlocks.DependencyInjection
{
	public class DependencyInjectionManager
	{
		private readonly IKernel _kernel;

		public DependencyInjectionManager()
		{
			// Create kernel.
			_kernel = new StandardKernel();
			_kernel.Load("*.dll");
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

		public object Get(Type serviceType)
		{
			return _kernel.Get(serviceType);
		}
	}
}