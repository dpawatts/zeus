using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Isis.ApplicationBlocks.DataMigrations.Console
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());

				ShowHeader();

				if (args == null || args.Length == 0)
				{
					ShowHelp();
					return;
				}

				switch (args[0].ToLower())
				{
					case "migrate":
						DoMigrate(args);
						break;
					default:
						ShowHelp();
						break;
				}
			}
			catch (ApplicationException) { }
			finally
			{
				System.Console.ResetColor();
			}
		}

		private static void ShowHeader()
		{
			System.Console.ForegroundColor = ConsoleColor.Cyan;
			System.Console.WriteLine("---------------------------");
			System.Console.WriteLine("ISIS Command Line Interface");
			System.Console.WriteLine("---------------------------");
			System.Console.WriteLine();
		}

		private static void ShowHelp()
		{
			System.Console.ForegroundColor = ConsoleColor.Red;
			System.Console.WriteLine("Parameter Options");
			System.Console.WriteLine();

			System.Console.WriteLine("MIGRATE");
			System.Console.WriteLine("isis migrate {action} {migrationsAssemblyPath} {connectionString} [{desiredVersion}]");
			System.Console.WriteLine("\t{action} = RunMigration, GetSetNames, ContainsMigrations");
			System.Console.WriteLine("\t{connectionString} = Database connection string");
			System.Console.WriteLine("\t{migrationsAssemblyPath} = Path of assembly which contains migrations");
			System.Console.WriteLine();
		}

		private static T GetArg<T>(string[] args, int index, string name, bool optional)
		{
			if (args == null || args.Length < index + 1)
			{
				if (optional)
					return default(T);

				System.Console.ForegroundColor = ConsoleColor.Red;
				System.Console.WriteLine(string.Format("Not enough parameters. Expected '{0}' at index {1}.", name, index));
				throw new ApplicationException();
			}

			T result = (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(args[index]);
			return result;
		}

		private static void DoMigrate(string[] args)
		{
			System.Console.WriteLine("MIGRATE");

			string migrationsAssemblyPath = GetArg<string>(args, 1, "migrationsAssemblyPath", false);
			string connectionString = GetArg<string>(args, 2, "connectionString", false);
			string setName = GetArg<string>(args, 3, "setName", false);
			int? desiredVersion = GetArg<int?>(args, 4, "desiredVersion", true);

			try
			{
				MigrationManager.Migrate(connectionString, migrationsAssemblyPath, setName, desiredVersion);

				System.Console.ForegroundColor = ConsoleColor.Green;
				System.Console.WriteLine("Successfully migrated");
			}
			catch (Exception ex)
			{
				System.Console.ForegroundColor = ConsoleColor.Red;
				System.Console.WriteLine(ex.Message);
				Exception innerException = ex.InnerException;
				while (innerException != null)
				{
					System.Console.WriteLine(innerException.Message);
					innerException = innerException.InnerException;
				}
			}
		}
	}
}