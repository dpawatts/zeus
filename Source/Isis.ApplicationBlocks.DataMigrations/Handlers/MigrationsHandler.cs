using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Isis.ApplicationBlocks.DataMigrations.Configuration;
using Isis.ApplicationBlocks.DataMigrations.Framework;

namespace Isis.ApplicationBlocks.DataMigrations.Handlers
{
	public class MigrationsHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			if (!context.Request.IsLocal)
				throw new Exception("Only accessible from local computer");

			Type baseMigrationType = typeof(Migration); Type migrationAttributeType = typeof(MigrationAttribute);

			context.Response.Write("<h1>Migration Assemblies</h1>");
			context.Response.Write("<table border='1'>");
			context.Response.Write("<tr>");
			context.Response.Write("<th>Assembly</th>");
			context.Response.Write("<th>Sets</th>");
			context.Response.Write("</tr>");

			foreach (AssemblyName assemblyName in GetMigrationAssemblyNames(baseMigrationType, migrationAttributeType))
			{
				context.Response.Write("<tr>");
				context.Response.Write(string.Format("<td>{0}</td>", assemblyName.FullName));
				context.Response.Write(string.Format("<td><a href=\"{0}?assembly={1}\">Sets</a></td>", context.Request.Path, assemblyName.FullName));
				context.Response.Write("</tr>");
			}

			context.Response.Write("</table>");

			if (context.Request.QueryString["assembly"] != null)
			{
				string assembly = context.Request.QueryString["assembly"];
				context.Response.Write(string.Format("<h2>{0}</h2>", assembly));

				context.Response.Write("<table border='1'>");
				context.Response.Write("<tr>");
				context.Response.Write("<th>Set</th>");
				context.Response.Write("<th>Database Version</th>");
				context.Response.Write("<th>Latest Version</th>");
				context.Response.Write("<th>Run</th>");
				context.Response.Write("</tr>");

				Assembly migrationAssembly = AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().FullName == assembly);
				MigrationsSection config = (MigrationsSection) ConfigurationManager.GetSection("isis.data/migrations");
				string connectionString = ConfigurationManager.ConnectionStrings[config.ConnectionStringName].ConnectionString;

				foreach (string setName in GetSetNames(baseMigrationType, migrationAttributeType, migrationAssembly))
				{
					context.Response.Write("<tr>");
					context.Response.Write(string.Format("<td>{0}</td>", setName));
					context.Response.Write(string.Format("<td>{0}</td>", MigrationManager.GetCurrentDatabaseVersion(setName, connectionString)));

					int latestVersion = MigrationManager.GetLatestVersionFromAssembly(migrationAssembly, baseMigrationType, migrationAttributeType, setName);
					context.Response.Write(string.Format("<td>{0}</td>", latestVersion));

					context.Response.Write("<td>");
					context.Response.Write("<form method=\"get\" style=\"margin:0\">");
					context.Response.Write(string.Format("<input type=\"hidden\" name=\"assembly\" value=\"{0}\" />", assembly));
					context.Response.Write(string.Format("<input type=\"hidden\" name=\"setName\" value=\"{0}\" />", setName));
					context.Response.Write(string.Format("<input type=\"text\" name=\"version\" style=\"width:30px\" value=\"{0}\" />", latestVersion));
					context.Response.Write("<input type=\"submit\" value=\"Migrate\" />");
					context.Response.Write("</form>");
					context.Response.Write("</td>");

					context.Response.Write("</tr>");
				}

				context.Response.Write("</table>");

				if (context.Request.QueryString["setName"] != null)
				{
					string setName = context.Request.QueryString["setName"];
					int version = Convert.ToInt32(context.Request.QueryString["version"]);

					context.Response.Write(string.Format("<h3>{0}</h3>", setName));

					StringWriter stringWriter = new StringWriter();
					TextWriterTraceListener traceListener = new TextWriterTraceListener(stringWriter);
					Trace.Listeners.Add(traceListener);
					MigrationManager.Migrate(connectionString, migrationAssembly.CodeBase, setName, version);
					context.Response.Write(stringWriter.ToString().Replace(Environment.NewLine, "<br />"));
					Trace.Listeners.Remove(traceListener);
				}
			}
		}

		private static List<AssemblyName> GetMigrationAssemblyNames(Type baseMigrationType, Type migrationAttributeType)
		{
			// Look in all assemblies loaded into the current AppDomain.
			List<AssemblyName> migrationAssemblyNames = new List<AssemblyName>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				// look for Migration classes in available DLLs
				foreach (Type migrationType in assembly.GetTypes().Where(t => baseMigrationType.IsAssignableFrom(t)))
				{
					if (HasMigrationAttribute(migrationType, migrationAttributeType))
					{
						migrationAssemblyNames.Add(assembly.GetName());
						break;
					}
				}
			}

			return migrationAssemblyNames;
		}

		private static bool HasMigrationAttribute(Type migrationType, Type migrationAttributeType)
		{
			int? version;
			return MigrationManager.HasMigrationAttribute(migrationType, migrationAttributeType, out version);
		}

		private static List<string> GetSetNames(Type baseMigrationType, Type migrationAttributeType, Assembly migrationAssembly)
		{
			// find all the unique namespaces which contain migration classes in the specified assembly
			List<string> setNames = new List<string>();
			foreach (Type type in migrationAssembly.GetTypes())
			{
				if (baseMigrationType.IsAssignableFrom(type))
					if (HasMigrationAttribute(type, migrationAttributeType))
						if (!setNames.Contains(type.Namespace))
							setNames.Add(type.Namespace);
			}

			return setNames;
		}
	}
}