using Isis.Reflection;
using Isis.Web;
using Isis.Web.Security;
using Ninject.Modules;
using Zeus.Admin;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.DynamicContent;
using Zeus.Globalization;
using Zeus.Installation;
using Zeus.Integrity;
using Zeus.Persistence;
using Zeus.Plugin;
using Zeus.Security;
using Zeus.Serialization;
using Zeus.Web;
using Zeus.Web.Security;
using IWebContext=Zeus.Web.IWebContext;

namespace Zeus.Engine
{
	public class ZeusModule : NinjectModule
	{
		public override void Load()
		{
			// Isis web security
			Bind<Isis.Web.IWebContext>().To<WebContext>().InSingletonScope();
			Bind<IAuthorizationService>().To<AuthorizationService>().InSingletonScope();
			Bind<ICredentialContextService>().To<CredentialContextService>().InSingletonScope();
			Bind<IAuthenticationContextService>().To<AuthenticationContextService>().InSingletonScope();
			Bind<IAuthenticationContextInitializer>().To<SecurityInitializer>().InSingletonScope();
			Bind<IAuthorizationInitializer>().To<SecurityInitializer>().InSingletonScope();

			// Admin
			Bind<IAdminManager>().To<AdminManager>().InSingletonScope();
			Bind<Navigator>().To<Navigator>().InSingletonScope();

			// Content Properties
			Bind<IContentPropertyManager>().To<ContentPropertyManager>().InSingletonScope();

			// Content Types
			Bind(typeof(AttributeExplorer<>)).To(typeof(AttributeExplorer<>)).InSingletonScope();
			Bind(typeof(IEditableHierarchyBuilder<>)).To(typeof(EditableHierarchyBuilder<>)).InSingletonScope();
			Bind<IContentTypeBuilder>().To<ContentTypeBuilder>().InSingletonScope();
			Bind<IContentTypeManager>().To<ContentTypeManager>().InSingletonScope();

			// Dynamic Content
			Bind<IDynamicContentManager>().To<DynamicContentManager>().InSingletonScope();

			// Engine
			Bind<IContentAdapterProvider>().To<ContentAdapterProvider>().InSingletonScope();
			Bind(typeof(IPluginFinder<>)).To(typeof(PluginFinder<>)).InSingletonScope();
			Bind<ITypeFinder>().To<TypeFinder>().InSingletonScope();

			// Globalization
			Bind<ILanguageManager>().To<LanguageManager>().InSingletonScope();

			// Installation
			Bind<InstallationManager>().To<InstallationManager>().InSingletonScope();

			// Integrity
			Bind<IIntegrityManager>().To<IntegrityManager>().InSingletonScope();
			Bind<IIntegrityEnforcer>().To<IntegrityEnforcer>().InSingletonScope();

			// Persistence
			Bind<IItemNotifier>().To<ItemNotifier>().InSingletonScope();
			Bind<IPersister>().To<ContentPersister>().InSingletonScope();
			Bind<IVersionManager>().To<VersionManager>().InSingletonScope();

			// Plugin
			Bind<IPluginBootstrapper>().To<PluginBootstrapper>().InSingletonScope();

			// Security
			Bind<ISecurityEnforcer>().To<SecurityEnforcer>().InSingletonScope();
			Bind<ISecurityManager>().To<SecurityManager>().InSingletonScope();

			// Serialization
			Bind<Exporter>().To<GZipExporter>().InSingletonScope();
			Bind<ItemXmlWriter>().To<ItemXmlWriter>().InSingletonScope();
			Bind<Importer>().To<GZipImporter>().InSingletonScope();
			Bind<ItemXmlReader>().To<ItemXmlReader>().InSingletonScope();

			// Web
			Bind<IErrorHandler>().To<ErrorHandler>().InSingletonScope();
			Bind<IHost>().To<Host>().InSingletonScope();
			Bind<IUrlParser>().To<MultipleSitesUrlParser>().InSingletonScope();
			Bind<IPermanentLinkManager>().To<PermanentLinkManager>().InSingletonScope();
			Bind<ICredentialRepository>().To<CredentialRepository>().InSingletonScope();
			Bind<IWebSecurityManager>().To<CredentialRepository>().InSingletonScope();
			Bind<PermissionDeniedHandler>().To<PermissionDeniedHandler>().InSingletonScope(); // FIX
			Bind<IRequestDispatcher>().To<RequestDispatcher>().InSingletonScope();
			Bind<IRequestLifecycleHandler>().To<RequestLifecycleHandler>().InSingletonScope();
			Bind<IWebContext>().To<WebRequestContext>().InSingletonScope();
		}
	}
}