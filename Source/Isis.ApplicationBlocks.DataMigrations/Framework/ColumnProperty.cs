using System;

namespace Isis.ApplicationBlocks.DataMigrations.Framework
{
	/// <summary>
	/// Represents a table column properties.
	/// </summary>
	[Flags]
	public enum ColumnProperties
	{
		None = 0,

		/// <summary>
		/// Null is allowable
		/// </summary>
		Null = 1,

		/// <summary>
		/// Null is not allowable
		/// </summary>
		NotNull = 2,

		/// <summary>
		/// Identity column, autoinc
		/// </summary>
		Identity = 4,

		/// <summary>
		/// Unique column
		/// </summary>
		Unique = 8,

		/// <summary>
		/// Primary Key
		/// </summary>
		PrimaryKey = 16 | NotNull,

		/// <summary>
		/// Primary key. Make the column a PrimaryKey and Identity
		/// </summary>
		PrimaryKeyWithIdentity = PrimaryKey | Identity
	}
}