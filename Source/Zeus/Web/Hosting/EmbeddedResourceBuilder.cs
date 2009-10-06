using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Routing;
using Isis.FrameworkBlocks.DependencyInjection;
using Ninject;

namespace Zeus.Web.Hosting
{
	public class EmbeddedResourceBuilder : IInitializable, IEmbeddedResourceBuilder
	{
		private readonly IKernel _kernel;
		private readonly ResourceSettings _resourceSettings;

		public EmbeddedResourceBuilder(IKernel kernel)
		{
			_kernel = kernel;
			_resourceSettings = new ResourceSettings();
		}

		public ResourceSettings ResourceSettings
		{
			get { return _resourceSettings; }
		}

		public void Initialize()
		{
			var searchPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath);
			IEnumerable<string> filenames = Directory.GetFiles(searchPath, "*.dll");
			DependencyInjectionUtility.RegisterAllComponents<IEmbeddedResourcePackage>(_kernel, filenames);

			foreach (IEmbeddedResourcePackage package in _kernel.GetAll<IEmbeddedResourcePackage>())
				package.Register(RouteTable.Routes, _resourceSettings);
		}
	}
}