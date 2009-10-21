using FluentMigrator;

namespace Zeus.Installation.Migrations
{
	[Migration(200910211042)]
	public class AddTables : Migration
	{
		public override void Up()
		{
			Create.Table("zeusItems")
				.WithIDColumn()
				.WithColumn("Type").AsString(500).NotNullable()
				.WithColumn("Created").AsDateTime().NotNullable()
				.WithColumn("Published").AsDateTime().Nullable()
				.WithColumn("Updated").AsDateTime().NotNullable()
				.WithColumn("Expires").AsDateTime().Nullable()
				.WithColumn("Name").AsString(250).Nullable()
				.WithColumn("ZoneName").AsString(50).Nullable()
				.WithColumn("Title").AsString(250).Nullable()
				.WithColumn("SortOrder").AsInt32().NotNullable()
				.WithColumn("Visible").AsBoolean().NotNullable()
				.WithColumn("SavedBy").AsString(50).Nullable()
				.WithColumn("VersionOfID").AsInt32().Nullable()
				.WithColumn("Version").AsInt32().NotNullable()
				.WithColumn("TranslationOfID").AsInt32().Nullable()
				.WithColumn("Language").AsString(50).Nullable()
				.WithColumn("ParentID").AsInt32().Nullable();

			Create.Table("zeusDetailCollections")
				.WithIDColumn()
				.WithColumn("ItemID").AsInt32().Nullable()
				.WithColumn("Name").AsString(50).NotNullable();

			Create.Table("zeusAuthorizationRules")
				.WithIDColumn()
				.WithColumn("ItemID").AsInt32().NotNullable()
				.WithColumn("Operation").AsString(50).NotNullable()
				.WithColumn("Role").AsString(50).Nullable()
				.WithColumn("User").AsString(50).Nullable()
				.WithColumn("Allowed").AsBoolean().NotNullable();

			Create.Table("zeusLanguageSettings")
				.WithIDColumn()
				.WithColumn("ItemID").AsInt32().NotNullable()
				.WithColumn("Language").AsString(50).NotNullable()
				.WithColumn("FallbackLanguage").AsString(50).Nullable();

			Create.Table("zeusDetails")
				.WithIDColumn()
				.WithColumn("Type").AsString(250).NotNullable()
				.WithColumn("ItemID").AsInt32().NotNullable()
				.WithColumn("DetailCollectionID").AsInt32().Nullable()
				.WithColumn("Name").AsString(50).Nullable()
				.WithColumn("BoolValue").AsBoolean().Nullable()
				.WithColumn("IntValue").AsInt32().Nullable()
				.WithColumn("LinkValue").AsInt32().Nullable()
				.WithColumn("DoubleValue").AsDouble().Nullable()
				.WithColumn("DateTimeValue").AsDateTime().Nullable()
				.WithColumn("StringValue").AsString(int.MaxValue).Nullable()
				.WithColumn("DetailBlobID").AsInt32().Nullable();

			Create.Table("zeusDetailBlobs")
				.WithIDColumn()
				.WithColumn("Blob").AsBinary(int.MaxValue).Nullable();

			Create.ForeignKey("FK_zeusDetailCollections_ItemID_zeusItems_ID")
				.FromTable("zeusDetailCollections").ForeignColumn("ItemID")
				.ToTable("zeusItems").PrimaryColumn("ID");

			Create.ForeignKey("FK_zeusAuthorizationRules_ItemID_zeusItems_ID")
				.FromTable("zeusAuthorizationRules").ForeignColumn("ItemID")
				.ToTable("zeusItems").PrimaryColumn("ID");

			Create.ForeignKey("FK_zeusLanguageSettings_ItemID_zeusItems_ID")
				.FromTable("zeusLanguageSettings").ForeignColumn("ItemID")
				.ToTable("zeusItems").PrimaryColumn("ID");

			Create.ForeignKey("FK_zeusDetails_ItemID_zeusItems_ID")
				.FromTable("zeusDetails").ForeignColumn("ItemID")
				.ToTable("zeusItems").PrimaryColumn("ID");

			Create.ForeignKey("FK_zeusDetails_DetailCollectionID_zeusDetailCollections_ID")
				.FromTable("zeusDetails").ForeignColumn("DetailCollectionID")
				.ToTable("zeusDetailCollections").PrimaryColumn("ID");

			Create.ForeignKey("FK_zeusDetails_DetailBlobID_zeusDetailBlobs_ID")
				.FromTable("zeusDetails").ForeignColumn("DetailBlobID")
				.ToTable("zeusDetailBlobs").PrimaryColumn("ID");

			Create.Index("IX_Language")
				.OnTable("zeusLanguageSettings")
				.OnColumn("ItemID").Ascending()
				.OnColumn("Language").Ascending()
				.WithOptions().Unique();
		}

		public override void Down()
		{
			Delete.Table("zeusDetailBlobs");
			Delete.Table("zeusDetails");
			Delete.Table("zeusLanguageSettings");
			Delete.Table("zeusAuthorizationRules");
			Delete.Table("zeusDetailCollections");
			Delete.Table("zeusItems");
		}
	}
}