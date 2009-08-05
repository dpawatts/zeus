namespace Zeus.Persistence.NH
{
	public interface IConfigurationBuilder
	{
		NHibernate.Cfg.Configuration Configuration
		{
			get;
		}
	}
}
