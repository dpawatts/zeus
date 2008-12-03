using System;

namespace Zeus.Persistence
{
	public interface IConfigurationBuilder
	{
		NHibernate.Cfg.Configuration Configuration
		{
			get;
		}
	}
}
