using FluentNHibernate;

namespace Zeus.Persistence.NH
{
	public class ZeusPersistenceModel : PersistenceModel
	{
		public ZeusPersistenceModel()
		{
			AddMappingsFromThisAssembly();
		}
	}
}
