using FluentMigrator.Builders.Create.Table;

namespace Zeus.Installation.Migrations
{
	internal static class MigrationExtensions
	{
		public static ICreateTableColumnOptionOrWithColumnSyntax WithIDColumn(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
		{
			return tableWithColumnSyntax
				.WithColumn("ID")
				.AsInt32()
				.NotNullable()
				.PrimaryKey()
				.Identity();
		}
	}
}