using System;
using System.Configuration;
using Castle.Windsor;
using Isis.ComponentModel;
using Isis.Web;
using Isis.Web.Security;
using Zeus.Admin;
using Zeus.Configuration;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.DynamicContent;
using Zeus.Globalization;
using Zeus.Installation;
using Zeus.Integrity;
using Zeus.Persistence;
using Zeus.Plugin;
using Zeus.Security;
using Zeus.Web;
using Zeus.Web.Security;
using IWebContext=Zeus.Web.IWebContext;

namespace Zeus.Engine
{
	public class ContentEngine
	{
		#region Properties

		public IAdminManager AdminManager
		{
			get { return Resolve<IAdminManager>(); }
		}

		public IFinder<ContentItem> Finder
		{
			get { return Resolve<IFinder<ContentItem>>(); }
		}

		public IContentTypeManager ContentTypes
		{
			get { return Resolve<IContentTypeManager>(); }
		}

		public ILanguageManager LanguageManager
		{
			get { return Resolve<ILanguageManager>(); }
		}

		public IPersister Persister
		{
			get { return Resolve<IPersister>(); }
		}

		public ISecurityManager SecurityManager
		{
			get { return Resolve<ISecurityManager>(); }
		}

		public IHost Host
		{
			get { return Resolve<IHost>(); }
		}

		public IUrlParser UrlParser
		{
			get { return Resolve<IUrlParser>(); }
		}

		public IWebContext WebContext
		{
			get { return Resolve<IWebContext>(); }
		}

		public IWindsorContainer Container
		{
			get { return IoC.Container; }
		}

		#endregion

		#region Constructor

		public ContentEngine(EventBroker eventBroker)
		{
			IoC.AddComponentInstance(eventBroker);
			IoC.AddComponentInstance(ConfigurationManager.GetSection("zeus/database") as DatabaseSection);
			HostSection hostSection = IoC.AddComponentInstance(ConfigurationManager.GetSection("zeus/host") as HostSection);
			IoC.AddComponentInstance(ConfigurationManager.GetSection("zeus/admin") as AdminSection);
			IoC.AddComponentInstance(ConfigurationManager.GetSection("zeus/dynamicContent") as DynamicContentSection);
			IoC.AddComponentInstance(ConfigurationManager.GetSection("zeus/globalization") as GlobalizationSection ?? new GlobalizationSection());

			SetupBasicServices();

			Url.DefaultExtension = hostSection.Web.Extension;
		}

		#endregion

		private static void SetupBasicServices()
		{
			// Isis web security
			IoC.SetupService<Isis.Web.IWebContext, WebContext>();
			IoC.SetupService<IAuthorizationService, AuthorizationService>();
			IoC.SetupService<ICredentialContextService, CredentialContextService>();
			IoC.SetupService<IAuthenticationContextService, AuthenticationContextService>();

			// Admin
			IoC.SetupService<IAdminManager, AdminManager>();
			IoC.SetupService<Navigator, Navigator>();

			// Content Properties
			IoC.SetupService<IContentPropertyManager, ContentPropertyManager>();

			// Content Types
			IoC.SetupService(typeof(AttributeExplorer<>), typeof(AttributeExplorer<>));
			IoC.SetupService(typeof(IEditableHierarchyBuilder<>), typeof(EditableHierarchyBuilder<>));
			IoC.SetupService<IContentTypeBuilder, ContentTypeBuilder>();
			IoC.SetupService<IContentTypeManager, ContentTypeManager>();

			// Dynamic Content
			IoC.SetupService<IDynamicContentManager, DynamicContentManager>();

			// Engine
			IoC.SetupService<IAspectControllerProvider, AspectControllerProvider>();
			//IoC.SetupService<IAssemblyFinder, AssemblyFinder>();
			IoC.SetupService(typeof(IPluginFinder<>), typeof(PluginFinder<>));
			IoC.SetupService<ITypeFinder, TypeFinder>();

			// Globalization
			IoC.SetupService<ILanguageManager, LanguageManager>();

			// Installation
			IoC.SetupService<InstallationManager, InstallationManager>();

			// Integrity
			IoC.SetupService<IIntegrityManager, IntegrityManager>();
			IoC.SetupService<IIntegrityEnforcer, IntegrityEnforcer>();

			// Persistence
			IoC.SetupService<IItemNotifier, ItemNotifier>();
			IoC.SetupService<IPersister, ContentPersister>();
			IoC.SetupService<IVersionManager, VersionManager>();

			// Plugin
			IoC.SetupService<IPluginBootstrapper, PluginBootstrapper>();

			// Security
			IoC.SetupService<ISecurityEnforcer, SecurityEnforcer>();
			IoC.SetupService<ISecurityManager, SecurityManager>();

			// Web
			IoC.SetupService<IErrorHandler, ErrorHandler>();
			IoC.SetupService<IHost, Host>();
			IoC.SetupService<IUrlParser, MultipleSitesUrlParser>();
			IoC.SetupService<IPermanentLinkManager, PermanentLinkManager>();
			IoC.SetupService<ICredentialRepository, CredentialRepository>();
			IoC.SetupService<IWebSecurityManager, CredentialRepository>();
			IoC.SetupService<PermissionDeniedHandler, PermissionDeniedHandler>(); // FIX
			IoC.SetupService<IRequestDispatcher, RequestDispatcher>();
			IoC.SetupService<IRequestLifecycleHandler, RequestLifecycleHandler>();
			IoC.SetupService<IWebContext, WebRequestContext>();
		}

		public void AddComponent(string key, Type serviceType, Type classType)
		{
			IoC.AddComponent(key, serviceType, classType);
		}

		public void AddComponent(string key, Type classType)
		{
			IoC.AddComponent(key, classType);
		}

		public void AddComponentInstance(string key, object instance)
		{
			IoC.AddComponentInstance(key, instance);
		}

		public void AddService<TService, TComponent>()
			where TComponent : TService
		{
			IoC.SetupService<TService, TComponent>();
		}

		public void Initialize()
		{
			IoC.AddComponentInstance(this);

			IPluginBootstrapper invoker = Resolve<IPluginBootstrapper>();
			invoker.InitializePlugins(this, invoker.GetPluginDefinitions());

			IoC.SetupPlugins();
			IoC.StartComponents();

			CredentialLocation rootCredentialLocation = new CredentialLocation
    	{
    		Repository = Resolve<ICredentialRepository>()
    	};
			Resolve<ICredentialContextService>().SetRootLocation(rootCredentialLocation);
		}

		public T Resolve<T>()
		{
			return IoC.Resolve<T>();
		}
	}
}
