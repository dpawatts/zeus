using Isis.ApplicationBlocks.DataMigrations.Framework;
using Smo = Microsoft.SqlServer.Management.Smo;

namespace Zeus.Installation.Migrations
{
	[Migration(2)]
	public class Migration002 : Migration
	{
		public override void Up()
		{
			AddTable("zeusDetailBlobs",
				new Column("ID", Smo.DataType.Int, ColumnProperties.PrimaryKeyWithIdentity),
				new Column("Blob", Smo.DataType.VarBinaryMax, ColumnProperties.Null));

			AddColumn("zeusDetails",
				new Column("DetailBlobID", Smo.DataType.Int, ColumnProperties.Null));

			AddForeignKey("zeusDetails", "DetailBlobID", "zeusDetailBlobs", "ID");

			RemoveColumn("zeusDetails", "Value");

			AddColumn("zeusAuthorizationRules",
				new Column("Allowed", Smo.DataType.Int, ColumnProperties.NotNull, 1));
		}

		public override void Down()
		{
			RemoveTable("zeusDetailBlobs");
		}
	}
}
