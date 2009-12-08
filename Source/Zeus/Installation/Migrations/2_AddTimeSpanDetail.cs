using FluentMigrator;

namespace Zeus.Installation.Migrations
{
	[Migration(200912081612)]
	public class AddTimeSpanDetail : Migration
	{
		public override void Up()
		{
			Create.Column("TimeSpanValue")
				.OnTable("zeusDetails")
				.AsTime().Nullable();
		}

		public override void Down()
		{
			Delete.Column("TimeSpanValue").FromTable("zeusDetails");
		}
	}
}