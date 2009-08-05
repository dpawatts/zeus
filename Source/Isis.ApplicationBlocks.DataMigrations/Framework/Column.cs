using Microsoft.SqlServer.Management.Smo;

namespace Isis.ApplicationBlocks.DataMigrations.Framework
{
	public class Column
	{
		#region Constructors

		public Column(string name, DataType type)
			: this(name, type, ColumnProperties.None)
		{

		}

		public Column(string name, DataType type, ColumnProperties columnProperties)
			: this(name, type, columnProperties, null)
		{
			
		}

		public Column(string name, DataType type, object defaultValue)
			: this(name, type, ColumnProperties.None, null)
		{

		}

		public Column(string name, DataType type, ColumnProperties columnProperties, object defaultValue)
		{
			this.Name = name;
			this.Type = type;
			this.ColumnProperties = columnProperties;
			this.DefaultValue = defaultValue;
		}

		#endregion

		#region Properties

		public string Name
		{
			get;
			set;
		}

		public DataType Type
		{
			get;
			set;
		}

		public ColumnProperties ColumnProperties
		{
			get;
			set;
		}

		public object DefaultValue
		{
			get;
			set;
		}

		public string Collation { get; set; }

		public bool IsIdentity
		{
			get { return (this.ColumnProperties & ColumnProperties.Identity) == ColumnProperties.Identity; }
		}

		public bool IsPrimaryKey
		{
			get { return (this.ColumnProperties & ColumnProperties.PrimaryKey) == ColumnProperties.PrimaryKey; }
		}

		public bool IsNullable
		{
			get
			{
				return ((this.ColumnProperties & ColumnProperties.Null) == ColumnProperties.Null)
				       || ((this.ColumnProperties & ColumnProperties.NotNull) != ColumnProperties.NotNull);
			}
		}

		#endregion
	}
}