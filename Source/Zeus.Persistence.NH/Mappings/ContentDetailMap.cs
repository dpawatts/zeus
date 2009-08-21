using FluentNHibernate.Mapping;
using Zeus.ContentProperties;

namespace Zeus.Persistence.NH.Mappings
{
	public class ContentDetailMap : ClassMap<PropertyData>
	{
		public ContentDetailMap()
		{
			WithTable("zeusDetails");

			Cache.AsReadWrite();

			Id(x => x.ID).WithUnsavedValue(0).GeneratedBy.Native();

			DiscriminateSubClassesOnColumn("Type")
				.SubClass<BooleanProperty>("Bool", m => m.Map(x => x.BoolValue, "BoolValue"))
				.SubClass<IntegerProperty>("Int", m => m.Map(x => x.IntegerValue, "IntValue"))
				.SubClass<LinkProperty>("Link", m => m.References(x => x.LinkedItem, "LinkValue").LazyLoad())
				.SubClass<DoubleProperty>("Double", m => m.Map(x => x.DoubleValue, "DoubleValue"))
				.SubClass<DateTimeProperty>("DateTime", m => m.Map(x => x.DateTimeValue, "DateTimeValue"))
				.SubClass<StringProperty>("String", m => m.Map(x => x.StringValue, "StringValue").CustomTypeIs("StringClob").WithLengthOf(1073741823))
				.SubClass<ObjectProperty>("Object", m => m.Map(x => x.Value, "Value").CustomTypeIs("Serializable"));//.WithLengthOf(2147483647));

			References(x => x.EnclosingItem, "ItemID").Not.Nullable().FetchType.Select();
			References(x => x.EnclosingCollection, "DetailCollectionID").FetchType.Select().LazyLoad();

			Map(x => x.Name).WithLengthOf(50);
		}
	}
}
