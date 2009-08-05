using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Isis.ApplicationBlocks.DataMigrations.Framework;
using Microsoft.SqlServer.Management.Common;
using Smo = Microsoft.SqlServer.Management.Smo;

namespace Isis.ApplicationBlocks.DataMigrations
{
	public static class MigrationManager
	{
		private const string MIGRATIONS_INFO_TABLE_NAME = "nmigrationMigrationsInfo";

		public static string CreateDatabase(string databaseServer, string databaseName)
		{
			return DatabaseManager.CreateDatabase(databaseServer, databaseName);
		}

		/// <summary>
		/// This version takes the version number from the assembly.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="migrationsAssemblyPath"></param>
		public static void Migrate(string connectionString, string migrationsAssemblyPath, string setName)
		{
			Migrate(connectionString, migrationsAssemblyPath, setName, null);
		}

		/// <summary>
		/// This version allows you to explicitly set the version.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="migrationsAssemblyPath"></param>
		/// <param name="desiredVersion"></param>
		public static void Migrate(string connectionString, string migrationsAssemblyPath, string setName, int? explicitDesiredVersion)
		{
			Migrate(connectionString, Assembly.LoadFrom(migrationsAssemblyPath), typeof(Migration), typeof(MigrationAttribute), setName, explicitDesiredVersion);
		}

		public static void Migrate(string connectionString, Assembly migrationAssembly, string setName)
		{
			Migrate(connectionString, migrationAssembly, typeof(Migration), typeof(MigrationAttribute), setName, null);
		}

		public static void Migrate(string connectionString, Assembly migrationAssembly, Type baseMigrationType, Type migrationAttributeType, string setName, int? explicitDesiredVersion)
		{
			// create database connection
			SqlConnection conn; ServerConnection serverConnection; Smo.Database database;
			DatabaseManager.GetSmoConnection(connectionString, out conn, out serverConnection, out database);

			// get assembly which contains migrations
			int desiredVersion;
			if (explicitDesiredVersion == null)
			{
				int latestVersion = GetLatestVersionFromAssembly(migrationAssembly, baseMigrationType, migrationAttributeType, setName);
				Trace.WriteLine(string.Format("Migrations assembly info: LatestVersion = '{0}'", latestVersion));
				desiredVersion = latestVersion;
			}
			else
			{
				desiredVersion = explicitDesiredVersion.Value;
			}

			try
			{
				// connect to db and begin a transaction
				serverConnection.Connect();
				Trace.WriteLine("Connected to server");
				serverConnection.BeginTransaction();
				Trace.WriteLine("Transaction begun");

				// check current version of database
				int currentVersion = GetCurrentDatabaseVersion(database, conn, setName);
				Trace.WriteLine(string.Format("Database current version is '{0}'", currentVersion));

				// if versions are the same, there's nothing to do!
				if (currentVersion == desiredVersion)
				{
					Trace.WriteLine("Database is already at desired version");
					serverConnection.RollBackTransaction();
					return;
				}

				// get all migrations from specified assembly and namespace
				SortedDictionary<int, Type> allMigrations = GetAllMigrations(migrationAssembly, baseMigrationType, migrationAttributeType, setName);

				// filter to just the migrations necessary to get from current version to desired version
				var filteredMigrations = allMigrations.Where(m => (currentVersion < desiredVersion) ? m.Key > currentVersion && m.Key <= desiredVersion : m.Key > desiredVersion);

				// order correctly and validate
				if (currentVersion > desiredVersion)
				{
					filteredMigrations = filteredMigrations.Reverse();

					if (!filteredMigrations.Any(m => m.Key == desiredVersion + 1))
						throw new Exception(string.Format("Could not find next version after desired version '{0}' in available migrations", desiredVersion));
				}
				else
				{
					if (!filteredMigrations.Any(m => m.Key == desiredVersion))
						throw new Exception(string.Format("Could not find desired version '{0}' in available migrations", desiredVersion));
				}

				// run the migrations
				foreach (var filteredMigration in filteredMigrations)
				{
					object migration = Activator.CreateInstance(filteredMigration.Value);
					baseMigrationType.GetProperty("Database").SetValue(migration, database, null);

					if (currentVersion < desiredVersion)
					{
						baseMigrationType.GetMethod("Up").Invoke(migration, null);
						Trace.WriteLine(string.Format("Up() called on migration version '{0}'", filteredMigration.Key));
					}
					else
					{
						baseMigrationType.GetMethod("Down").Invoke(migration, null);
						Trace.WriteLine(string.Format("Down() called on migration version '{0}'", filteredMigration.Key));
					}
				}

				// update the database version
				UpdateDatabaseVersion(conn, setName, desiredVersion);
				Trace.WriteLine(string.Format("Database version updated from '{0}' to '{1}'", currentVersion, desiredVersion));

				// commit the transaction
				serverConnection.CommitTransaction();
				Trace.WriteLine("Transaction committed");
			}
			catch (Exception ex)
			{
				// if any error occurred, roll back transaction
				serverConnection.RollBackTransaction();
				Trace.WriteLine("Transaction rolled back");

				throw new Exception("Database migration failed: " + ex.ToString(), ex);
			}
			finally
			{
				// clean up by closing db connection
				if (serverConnection.IsOpen)
				{
					serverConnection.Disconnect();
					Trace.WriteLine("Disconnected from server");
				}
			}
		}

		public static int GetLatestVersionFromAssembly(Assembly migrationAssembly, Type baseMigrationType, Type migrationAttributeType, string setName)
		{
			// get latest version by looking in the assembly, using the _setName as the namespace
			SortedDictionary<int, Type> migrations = GetAllMigrations(migrationAssembly, baseMigrationType, migrationAttributeType, setName);
			return (migrations.Count > 0) ? migrations.Last().Key : 0;
		}

		public static int GetCurrentDatabaseVersion(string setName, string connectionString)
		{
			SqlConnection conn; ServerConnection serverConnection; Smo.Database database;
			DatabaseManager.GetSmoConnection(connectionString, out conn, out serverConnection, out database);
			return GetCurrentDatabaseVersion(database, conn, setName);
		}

		#region Helper methods

		public static bool HasMigrationAttribute(Type migrationType, Type migrationAttributeType, out int? version)
		{
			object migrationAttribute = Attribute.GetCustomAttribute(migrationType, migrationAttributeType);
			if (migrationAttribute != null)
			{
				version = (int) migrationAttributeType.GetProperty("Version").GetValue(migrationAttribute, null);
				return true;
			}
			else
			{
				version = null;
				return false;
			}
		}

		private static SortedDictionary<int, Type> GetAllMigrations(Assembly migrationAssembly, Type baseMigrationType, Type migrationAttributeType, string setName)
		{
			SortedDictionary<int, Type> migrations = new SortedDictionary<int, Type>();

			// find all the migration classes in the specified assembly that are contained within the specified namespace
			Type[] types = migrationAssembly.GetTypes();
			foreach (Type type in types)
			{
				if (type.Namespace == setName && baseMigrationType.IsAssignableFrom(type))
				{
					int? version;
					if (HasMigrationAttribute(type, migrationAttributeType, out version))
					{
						// store the type in an ordered dictionary, with the version number as the key
						migrations.Add(version.Value, type);
					}
				}
			}

			return migrations;
		}

		private static int GetCurrentDatabaseVersion(Smo.Database database, SqlConnection conn, string setName)
		{
			// first, check that schema info table exists - if not, create it
			if (!database.Tables.Contains(MIGRATIONS_INFO_TABLE_NAME))
			{
				Trace.WriteLine(string.Format("Table '{0}' does not exist; creating it", MIGRATIONS_INFO_TABLE_NAME));

				// create table
				Smo.Table schemaInfoTable = new Smo.Table(database, MIGRATIONS_INFO_TABLE_NAME);
				Smo.Column setNameColumn = new Smo.Column(schemaInfoTable, "SetName", Smo.DataType.VarChar(200));
				setNameColumn.Nullable = false;
				Smo.Column versionColumn = new Smo.Column(schemaInfoTable, "Version", Smo.DataType.Int);
				versionColumn.Nullable = false;
				schemaInfoTable.Columns.Add(setNameColumn);
				schemaInfoTable.Columns.Add(versionColumn);
				schemaInfoTable.Create();

				// insert a single row and insert version '0' into it
				SqlCommand command = new SqlCommand();
				command.Connection = conn;
				command.CommandText = string.Format("INSERT INTO {0} VALUES ('{1}', 0)", MIGRATIONS_INFO_TABLE_NAME, setName);
				command.ExecuteNonQuery();
			}

			// then, get the current version
			SqlDataReader reader = null;

			try
			{
				SqlCommand command = new SqlCommand();
				command.Connection = conn;
				command.CommandText = string.Format("SELECT Version FROM {0} WHERE SetName = '{1}'", MIGRATIONS_INFO_TABLE_NAME, setName);

				reader = command.ExecuteReader();
				if (reader.Read())
				{
					return reader.GetInt32(0);
				}
				else
				{
					reader.Close();

					// insert record into table for this set
					command.CommandText = string.Format("INSERT INTO {0} VALUES ('{1}', 0)", MIGRATIONS_INFO_TABLE_NAME, setName);
					command.ExecuteNonQuery();

					return 0;
				}
			}
			finally
			{
				if (reader != null && !reader.IsClosed)
					reader.Close();
			}
		}

		private static void UpdateDatabaseVersion(SqlConnection conn, string setName, int newVersion)
		{
			SqlCommand command = new SqlCommand();
			command.Connection = conn;
			command.CommandText = string.Format("UPDATE {0} SET Version = @Version WHERE SetName = '{1}'", MIGRATIONS_INFO_TABLE_NAME, setName);
			command.Parameters.AddWithValue("@Version", newVersion);

			command.ExecuteNonQuery();
		}

		#endregion
	}
}