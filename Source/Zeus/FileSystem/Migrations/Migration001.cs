using System;
using SoundInTheory.NMigration.Framework;
using Smo = Microsoft.SqlServer.Management.Smo;

namespace Zeus.FileSystem.Migrations
{
	[Migration(1)]
	public class Migration001 : Migration
	{
		public override void Up()
		{
			AddTable("zeusFileSystemFolders",
				new Column("ID", Smo.DataType.Int, ColumnProperties.PrimaryKeyWithIdentity),
				new Column("Name", Smo.DataType.NVarChar(500), ColumnProperties.NotNull),
				new Column("ParentID", Smo.DataType.Int, ColumnProperties.Null));

			AddTable("zeusFileSystemFiles",
				new Column("ID", Smo.DataType.Int, ColumnProperties.PrimaryKeyWithIdentity),
				new Column("FolderID", Smo.DataType.Int, ColumnProperties.NotNull),
				new Column("Name", Smo.DataType.NVarChar(500), ColumnProperties.NotNull),
				new Column("Data", Smo.DataType.VarBinaryMax, ColumnProperties.Null));

			AddForeignKey("zeusFileSystemFolders", "ParentID", "zeusFileSystemFolders", "ID");
			AddForeignKey("zeusFileSystemFiles", "FolderID", "zeusFileSystemFolders", "ID");
		}

		public override void Down()
		{
			RemoveTable("zeusFileSystemFiles");
			RemoveTable("zeusFileSystemFolders");
		}
	}
}
