using FluentNHibernate.Mapping;
using Zeus.Security;

namespace Zeus.Persistence.NH.Mappings
{
	public class AuthorizationRuleMap : ClassMap<AuthorizationRule>
	{
		public AuthorizationRuleMap()
		{
			WithTable("zeusAuthorizationRules");

			Cache.AsNonStrictReadWrite();

			Id(x => x.ID).WithUnsavedValue(0).GeneratedBy.Native();

			Map(x => x.Operation).Not.Nullable().WithLengthOf(50);
			Map(x => x.Role).WithLengthOf(50);
			Map(x => x.User).WithLengthOf(50);

			References(x => x.EnclosingItem, "ItemID").Not.Nullable();
		}
	}
}
