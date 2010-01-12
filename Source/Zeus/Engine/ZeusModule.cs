using Ninject.Modules;
using Zeus.Admin;
using Zeus.BaseLibrary.Reflection;
using Zeus.BaseLibrary.Web;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.DynamicContent;
using Zeus.FileSystem;
using Zeus.Globalization;
using Zeus.Installation;
using Zeus.Integrity;
using Zeus.Net;
using Zeus.Persistence;
using Zeus.Plugin;
using Zeus.Security;
using Zeus.Serialization;
using Zeus.Web;
using Zeus.Web.Hosting;
using Zeus.Web.Security;
using IWebContext=Zeus.Web.IWebContext;

namespace Zeus.Engine
{
	public class ZeusModule : NinjectModule
	{
		public override void Load()
		{
			// Admin
			Bind<IContentManager>().To<ContentManager>().InSingletonScope();
			
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
			Bind<ContentTypeConfigurationService>().ToSelf().InSingletonScope();

			// Dynamic Content
			Bind<IDynamicContentManager>().To<DynamicContentManager>().InSingletonScope();

			// Engine
			Bind<IContentAdapterProvider>().To<ContentAdapterProvider>().InSingletonScope();
			Bind(typeof(IPluginFinder<>)).To(typeof(PluginFinder<>)).InSingletonScope();
			Bind<ITypeFinder>().To<TypeFinder>().InSingletonScope();

			// File System
			Bind<IFileSystemService>().To<FileSystemService>().InSingletonScope();

			// Globalization
			Bind<ILanguageManager>().To<LanguageManager>().InSingletonScope();

			// Installation
			Bind<InstallationManager>().To<InstallationManager>().InSingletonScope();

			// Integrity
			Bind<IIntegrityManager>().To<IntegrityManager>().InSingletonScope();
			Bind<IIntegrityEnforcer>().To<IntegrityEnforcer>().InSingletonScope();

			// Net
			Bind<IHttpClient>().To<HttpClient>().InSingletonScope();

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
			Bind<ICredentialStore>().To<CredentialStore>().InSingletonScope();
			Bind<IWebSecurityManager>().To<CredentialStore>().InSingletonScope();
			Bind<PermissionDeniedHandler>().To<PermissionDeniedHandler>().InSingletonScope(); // FIX
			Bind<IRequestDispatcher>().To<RequestDispatcher>().InSingletonScope();
			Bind<IRequestLifecycleHandler>().To<RequestLifecycleHandler>().InSingletonScope();
			Bind<IWebContext>().To<WebRequestContext>().InSingletonScope();
			Bind<IEmbeddedResourceBuilder>().To<EmbeddedResourceBuilder>().InSingletonScope();
			Bind<IEmbeddedResourceManager>().To<EmbeddedResourceManager>().InSingletonScope();

			// Web security
			Bind<BaseLibrary.Web.IWebContext>().To<WebContext>().InSingletonScope();
			Bind<IAuthorizationService>().To<AuthorizationService>().InSingletonScope();
			Bind<ICredentialStore>().To<CredentialStore>().InSingletonScope();
			Bind<ICredentialService>().To<CredentialService>().InSingletonScope();
			Bind<IAuthenticationContextService>().To<AuthenticationContextService>().InSingletonScope();
			Bind<IAuthenticationContextInitializer>().To<SecurityInitializer>().InSingletonScope();
			Bind<IAuthorizationInitializer>().To<SecurityInitializer>().InSingletonScope();
			Bind<IWebSecurityService>().To<WebSecurityService>().InSingletonScope();
		}
	}
}