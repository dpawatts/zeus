using Isis.ComponentModel;

namespace Zeus.Persistence.NH
{
	public interface IConfigurationBuilder : IService
	{
		NHibernate.Cfg.Configuration Configuration
		{
			get;
		}
	}
}
