using System;
using Zeus.Linq;
using Zeus.Persistence;
using Zeus.Definitions;
using Zeus.Web;
using System.Configuration;
using Zeus.Configuration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;
using Castle.Windsor.Installer;
using Castle.MicroKernel;

namespace Zeus.Engine
{
	public class ContentEngine
	{
		private IWindsorContainer _container;

		#region Properties

		public ContentContext Database
		{
			get { return new ContentContext(_container.Resolve<ISessionProvider>()); }
		}

		public IDefinitionManager Definitions
		{
			get { return _container.Resolve<IDefinitionManager>(); }
		}

		public IPersister Persister
		{
			get { return _container.Resolve<IPersister>(); }
		}

		public Host Host
		{
			get { return new Host((HostSection) ConfigurationManager.GetSection("zeus/host")); }
		}

		#endregion

		#region Constructor

		public ContentEngine()
		{
			AssemblyResource resource = new AssemblyResource("assembly://Zeus/Configuration/castle.config");

			_container = new WindsorContainer();

			XmlInterpreter interpreter = new XmlInterpreter(resource);
			interpreter.ProcessResource(resource, _container.Kernel.ConfigurationStore);

			DefaultComponentInstaller installer = new DefaultComponentInstaller();
			installer.SetUp(_container, _container.Kernel.ConfigurationStore);
		}

		#endregion
	}
}
