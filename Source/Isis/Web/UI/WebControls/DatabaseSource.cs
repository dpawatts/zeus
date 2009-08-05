using System;
using System.Configuration;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace Isis.Web.UI.WebControls
{
	[Serializable]
	public class DatabaseSource
	{
		[Category("Data"), DefaultValue("")]
		public string ConnectionStringName
		{
			get;
			set;
		}

		[Category("Data"), DefaultValue("")]
		public string TableName
		{
			get;
			set;
		}

		[Category("Data"), DefaultValue("")]
		public string ColumnName
		{
			get;
			set;
		}

		[Category("Data"), TypeConverter(typeof(StringArrayConverter))]
		public string[] PrimaryKeyNames
		{
			get;
			set;
		}

		[Category("Data"), DefaultValue("")]
		public string PrimaryKeyName
		{
			get { return (this.PrimaryKeyNames != null && this.PrimaryKeyNames.Length > 0) ? this.PrimaryKeyNames[0] : string.Empty; }
			set { this.PrimaryKeyNames = new string[] { value }; }
		}

		[Category("Data"), TypeConverter(typeof(StringArrayConverter))]
		public string[] PrimaryKeyValues
		{
			get;
			set;
		}

		[Category("Data"), DefaultValue("")]
		public string PrimaryKeyValue
		{
			get { return (this.PrimaryKeyValues != null && this.PrimaryKeyValues.Length > 0) ? this.PrimaryKeyValues[0] : string.Empty; }
			set { this.PrimaryKeyValues = new string[] { value }; }
		}

		public DatabaseSource()
		{
			this.ConnectionStringName = string.Empty;
			this.TableName = string.Empty;
			this.ColumnName = string.Empty;
			this.PrimaryKeyName = string.Empty;
			this.PrimaryKeyValue = string.Empty;
		}

		public bool CheckFileExists(out string fileName)
		{
			fileName = null;

			SqlConnection conn = null; SqlDataAdapter adapter = null; DataTable dt = null;
			try
			{
				string connectionString = ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString;
				conn = new SqlConnection(connectionString);
				conn.Open();

				string whereFilter = string.Empty;
				for (int i = 0, length = this.PrimaryKeyNames.Length; i < length; ++i)
				{
					string primaryKeyName = this.PrimaryKeyNames[i];
					string primaryKeyValue = this.PrimaryKeyValues[i];
					whereFilter += string.Format("{0} = {1}", primaryKeyName, primaryKeyValue);
					if (i < length - 1)
						whereFilter += " AND ";
				}

				string selectSql = string.Format("SELECT CAST((CASE WHEN [{0}] IS NULL THEN 1 ELSE 0 END) AS BIT), {0}FileName FROM {1} WHERE {2}",
					this.ColumnName, this.TableName, whereFilter);

				adapter = new SqlDataAdapter(selectSql, conn);

				dt = new DataTable();
				adapter.Fill(dt);

				if (dt.Rows.Count != 1)
					throw new InvalidOperationException("Could not retrieve value from database");

				if (dt.Rows[0].Field<bool>(0))
				{
					return false; // this is fine, just means there's no value in the database
				}
				else
				{
					fileName = dt.Rows[0].Field<string>(1);
					return true;
				}
			}
			finally
			{
				if (dt != null)
					dt.Dispose();
				if (adapter != null)
					adapter.Dispose();
				if (conn != null)
				{
					conn.Close();
					conn.Dispose();
				}
			}
		}

		public byte[] GetBytes(out string mimeType, out string fileName)
		{
			mimeType = fileName = null;

			SqlConnection conn = null; SqlDataAdapter adapter = null; DataTable dt = null;
			try
			{
				string connectionString = ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString;
				conn = new SqlConnection(connectionString);
				conn.Open();

				string whereFilter = string.Empty;
				for (int i = 0, length = this.PrimaryKeyNames.Length; i < length; ++i)
				{
					string primaryKeyName = this.PrimaryKeyNames[i];
					string primaryKeyValue = this.PrimaryKeyValues[i];
					whereFilter += string.Format("{0} = {1}", primaryKeyName, primaryKeyValue);
					if (i < length - 1)
						whereFilter += " AND ";
				}

				string selectSql = string.Format("SELECT [{0}], {0}MimeType, {0}FileName FROM {1} WHERE {2}",
					this.ColumnName, this.TableName, whereFilter);

				adapter = new SqlDataAdapter(selectSql, conn);

				dt = new DataTable();
				adapter.Fill(dt);

				if (dt.Rows.Count != 1)
					throw new InvalidOperationException("Could not retrieve value from database");

				object value = dt.Rows[0][0];

				if (value == null || value is DBNull)
					return null; // this is fine, just means there's no value in the database

				if (!(value is byte[]))
					throw new InvalidOperationException(string.Format("Expected object of type '{0}' but found object of type '{1}'", typeof(byte[]).FullName, value.GetType().FullName));

				mimeType = dt.Rows[0].Field<string>(1);
				fileName = dt.Rows[0].Field<string>(2);

				return (byte[]) value;
			}
			finally
			{
				if (dt != null)
					dt.Dispose();
				if (adapter != null)
					adapter.Dispose();
				if (conn != null)
				{
					conn.Close();
					conn.Dispose();
				}
			}
		}
	}
}
