using System;
using SoundInTheory.NMigration.Framework;
using Smo = Microsoft.SqlServer.Management.Smo;

namespace Zeus.Install.Migrations
{
	[Migration(1)]
	public class Migration001 : Migration
	{
		public override void Up()
		{
			AddTable("zeusItems",
				new Column("ID", Smo.DataType.Int, ColumnProperties.PrimaryKeyWithIdentity),
				new Column("Type", Smo.DataType.NVarChar(500), ColumnProperties.NotNull),
				new Column("Created", Smo.DataType.DateTime, ColumnProperties.NotNull),
				new Column("Published", Smo.DataType.DateTime, ColumnProperties.Null),
				new Column("Updated", Smo.DataType.DateTime, ColumnProperties.NotNull),
				new Column("Expires", Smo.DataType.DateTime, ColumnProperties.Null),
				new Column("Name", Smo.DataType.NVarChar(250), ColumnProperties.Null),
				new Column("ZoneName", Smo.DataType.NVarChar(50), ColumnProperties.Null),
				new Column("Title", Smo.DataType.NVarChar(250), ColumnProperties.Null),
				new Column("SortOrder", Smo.DataType.Int, ColumnProperties.NotNull),
				new Column("Visible", Smo.DataType.Bit, ColumnProperties.NotNull),
				new Column("SavedBy", Smo.DataType.NVarChar(50), ColumnProperties.Null),
				new Column("VersionOfID", Smo.DataType.Int, ColumnProperties.Null),
				new Column("ParentID", Smo.DataType.Int, ColumnProperties.Null));
			/*
			
CREATE TABLE [dbo].[zeusDetailCollections](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[zeusDetailCollections]  WITH CHECK ADD  CONSTRAINT [FKBE85C49A6AB296072] FOREIGN KEY([ItemID])
REFERENCES [dbo].[zeusItems] ([ID])
GO

ALTER TABLE [dbo].[zeusDetailCollections] CHECK CONSTRAINT [FKBE85C49A6AB296072]
GO


			 * */
		}
	}
}
