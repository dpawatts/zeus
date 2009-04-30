using FluentNHibernate.Mapping;
using Zeus.ContentProperties;

namespace Zeus.Persistence.NH.Mappings
{
	public class DetailCollectionMap : ClassMap<PropertyCollection>
	{
		public DetailCollectionMap()
		{
			WithTable("zeusDetailCollections");

			Cache.AsReadWrite();

			Id(x => x.ID).WithUnsavedValue(0).GeneratedBy.Native();

			References(x => x.EnclosingItem, "ItemID");

			Map(x => x.Name).Not.Nullable().WithLengthOf(50);

			HasMany(x => x.Details)
				.AsBag()
				.Cascade.AllDeleteOrphan()
				.Inverse()
				// cache usage=nonsrict-read-write
				.WithKeyColumn("DetailCollectionID");
		}
	}
}
