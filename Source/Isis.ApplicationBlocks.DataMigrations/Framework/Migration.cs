using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Smo;
using Smo = Microsoft.SqlServer.Management.Smo;

namespace Isis.ApplicationBlocks.DataMigrations.Framework
{
	public abstract class Migration
	{
		#region Properties

		public Smo.Database Database
		{
			get;
			internal set;
		}

		#endregion

		#region Methods

		public virtual void Up() { }
		public virtual void Down() { }

		#region API Methods

		protected void AddTable(string name, params Column[] columns)
		{
			Smo.Table dbTable = new Smo.Table(this.Database, name);

			foreach (Column column in columns)
				AddColumn(dbTable, column);

			AddPrimaryKey(columns, dbTable);

			dbTable.Create();
		}

		private void AddColumn(Table dbTable, Column column)
		{
			Smo.Column dbColumn = new Smo.Column(dbTable, column.Name, column.Type);

			if (column.DefaultValue != null)
			{
				Smo.DefaultConstraint defaultConstraint = dbColumn.AddDefaultConstraint();
				defaultConstraint.Text = GetDefaultValue(column.DefaultValue);
			}

			if (!string.IsNullOrEmpty(column.Collation))
				dbColumn.Collation = column.Collation;

			if (column.IsIdentity)
			{
				dbColumn.Identity = true;
				dbColumn.IdentityIncrement = 1;
			}

			dbColumn.Nullable = column.IsNullable;

			if ((column.ColumnProperties & ColumnProperties.Unique) == ColumnProperties.Unique)
				AddUniqueIndex(column, dbTable);

			dbTable.Columns.Add(dbColumn);
		}

		private static string GetDefaultValue(object defaultValue)
		{
			if (defaultValue is bool)
				defaultValue = ((bool) defaultValue) ? 1 : 0;
			return defaultValue.ToString();
		}

		protected void AddForeignKey(string table, string columnName, string referencedTableName, string referencedColumnName, bool cascadeDelete)
		{
			Smo.Table dbTable = GetTable(table);

			Smo.ForeignKey foreignKey = new Smo.ForeignKey(dbTable, GetForeignKeyName(dbTable.Name, referencedTableName));
			foreignKey.Columns.Add(new Smo.ForeignKeyColumn(foreignKey, columnName, referencedColumnName));
			foreignKey.ReferencedTable = referencedTableName;
			if (cascadeDelete)
				foreignKey.DeleteAction = Smo.ForeignKeyAction.Cascade;

			foreignKey.Create();
		}

		/// <summary>
		/// Creates a foreign key on the specified tables.
		/// </summary>
		/// <param name="table">The table instance that the foreign key should be created on.</param>
		/// <param name="columnName">The column name which is to be linked to the primary key of the table specified in referencedTableName.</param>
		/// <param name="referencedTable">The table instance that contains the primary key.</param>
		/// <returns></returns>
		protected void AddForeignKey(string table, string columnName, string referencedTableName, string referencedColumnName)
		{
			AddForeignKey(table, columnName, referencedTableName, referencedColumnName, false);
		}

		private static void AddUniqueIndex(Column column, Smo.Table dbTable)
		{
			Smo.Index index = new Smo.Index(dbTable, "IX_" + dbTable.Name);
			index.IndexKeyType = Smo.IndexKeyType.DriUniqueKey;

			index.IndexedColumns.Add(new Smo.IndexedColumn(index, column.Name));

			dbTable.Indexes.Add(index);
		}

		protected void AddUniqueKey(string table, params string[] columnNames)
		{
			Smo.Table dbTable = GetTable(table);

			Smo.Index index = new Smo.Index(dbTable, "IX_" + table + "_" + string.Join("_", columnNames))
    	{
    		IndexKeyType = Smo.IndexKeyType.DriUniqueKey
    	};

			foreach (string columnName in columnNames)
				index.IndexedColumns.Add(new Smo.IndexedColumn(index, columnName));

			dbTable.Indexes.Add(index);

			index.Create();
		}

		protected void RemoveTable(string name)
		{
			GetTable(name).Drop();
		}

		protected void RemoveColumn(string table, string name)
		{
			GetTable(table).Columns[name].Drop();
		}

		protected void RemoveForeignKey(string table, string referencedTableName)
		{
			Smo.Table dbTable = GetTable(table);
			dbTable.ForeignKeys[GetForeignKeyName(dbTable.Name, referencedTableName)].Drop();
		}

		/// <summary>
		/// Add a column to an existing table
		/// </summary>
		/// <param name="table">The name of the table that will get the new column</param>
		/// <param name="column">The new column definition</param>
		protected void AddColumn(string table, Column column)
		{
			Smo.Table dbTable = GetTable(table);
			AddColumn(dbTable, column);
			dbTable.Alter();
		}

		#endregion

		#region Helper methods

		private Smo.Table GetTable(string name)
		{
			return this.Database.Tables[name];
		}

		private static void AddPrimaryKey(Column[] columns, Smo.Table dbTable)
		{
			IEnumerable<Column> primaryKeyColumns = columns.Where(c => c.IsPrimaryKey);
			if (primaryKeyColumns.Any())
			{
				Smo.Index index = new Smo.Index(dbTable, "PK_" + dbTable.Name);
				index.IndexKeyType = Smo.IndexKeyType.DriPrimaryKey;

				foreach (Column primaryKeyColumn in primaryKeyColumns)
					index.IndexedColumns.Add(new Smo.IndexedColumn(index, primaryKeyColumn.Name));

				dbTable.Indexes.Add(index);
			}
		}

		private static string GetForeignKeyName(string tableName, string referencedTableName)
		{
			return string.Format("FK_{0}_{1}", tableName, referencedTableName);
		}

		#endregion

		#endregion
	}
}