using Ninject.Modules;

namespace Zeus.Persistence.NH
{
	public class PersistenceModule : NinjectModule
	{
		public override void Load()
		{
			Bind(typeof(IRepository<,>)).To(typeof(Repository<,>)).InSingletonScope();
			Bind(typeof(IFinder<>)).To(typeof(Finder<>)).InSingletonScope();
			Bind<IConfigurationBuilder>().To<ConfigurationBuilder>().InSingletonScope();
			Bind<ISessionProvider>().To<SessionProvider>().InSingletonScope();
			Bind<INotifyingInterceptor>().To<NotifyingInterceptor>().InSingletonScope();
		}
	}
}