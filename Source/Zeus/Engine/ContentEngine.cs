using System;
using Zeus.Linq;
using Zeus.Persistence;
using Zeus.ContentTypes;
using Zeus.Web;
using System.Configuration;
using Zeus.Configuration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;
using Castle.Windsor.Installer;
using Castle.MicroKernel;
using Zeus.Security;

namespace Zeus.Engine
{
	public class ContentEngine
	{
		private IWindsorContainer _container;

		#region Properties

		public IItemFinder Finder
		{
			get { return _container.Resolve<IItemFinder>(); }
		}

		public IContentTypeManager ContentTypes
		{
			get { return _container.Resolve<IContentTypeManager>(); }
		}

		public IPersister Persister
		{
			get { return _container.Resolve<IPersister>(); }
		}

		public ISecurityManager SecurityManager
		{
			get { return _container.Resolve<ISecurityManager>(); }
		}

		public Host Host
		{
			get { return _container.Resolve<Host>(); }
		}

		public IUrlParser UrlParser
		{
			get { return _container.Resolve<IUrlParser>(); }
		}

		public IWebContext WebContext
		{
			get { return _container.Resolve<IWebContext>(); }
		}

		#endregion

		#region Constructor

		public ContentEngine()
		{
			AssemblyResource resource = new AssemblyResource("assembly://Zeus/Configuration/castle.config");

			_container = new WindsorContainer();

			AddComponentInstance<DatabaseSection>(ConfigurationManager.GetSection("zeus/database") as DatabaseSection);
			AddComponentInstance<Host>(new Host((HostSection) ConfigurationManager.GetSection("zeus/host")));

			XmlInterpreter interpreter = new XmlInterpreter(resource);
			interpreter.ProcessResource(resource, _container.Kernel.ConfigurationStore);

			DefaultComponentInstaller installer = new DefaultComponentInstaller();
			installer.SetUp(_container, _container.Kernel.ConfigurationStore);
		}

		#endregion

		public T Resolve<T>()
		{
			return _container.Resolve<T>();
		}

		public void AddComponentInstance<T>(T instance)
		{
			if (instance != null)
				_container.Kernel.AddComponentInstance(typeof(T).FullName, typeof(T), instance);
		}
	}
}
