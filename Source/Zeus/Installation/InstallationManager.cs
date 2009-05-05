using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Isis.ApplicationBlocks.DataMigrations;
using Isis.Web.Security;
using Zeus.Configuration;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Installation.Migrations;
using Zeus.Persistence;
using Zeus.Web;
using AuthorizationRule=Zeus.Security.AuthorizationRule;
using PropertyCollection=System.Data.PropertyCollection;

namespace Zeus.Installation
{
	/// <summary>
	/// Wraps functionality to request database status and generate n2's 
	/// database schema on multiple database flavours.
	/// </summary>
	public class InstallationManager
	{
		private readonly IContentTypeManager _contentTypeManager;
		//Importer importer;
		private readonly IPersister persister;
		private readonly IFinder<ContentItem> _contentItemFinder;
		private readonly IFinder<PropertyData> _contentDetailFinder;
		private readonly IFinder<PropertyCollection> _detailCollectionFinder;
		private readonly IFinder<AuthorizationRule> _authorizationRuleFinder;
		private readonly IHost host;
		private readonly ICredentialContextService _credentialContextService;

		public InstallationManager(IHost host, IContentTypeManager contentTypeManager, /*Importer importer, */IPersister persister,
			IFinder<ContentItem> contentItemFinder, IFinder<PropertyData> contentDetailFinder,
			IFinder<PropertyCollection> detailCollectionFinder, IFinder<AuthorizationRule> authorizationRuleFinder,
			ICredentialContextService credentialContextService)
		{
			this.host = host;
			_contentTypeManager = contentTypeManager;
			//this.importer = importer;
			this.persister = persister;
			_contentItemFinder = contentItemFinder;
			_contentDetailFinder = contentDetailFinder;
			_detailCollectionFinder = detailCollectionFinder;
			_authorizationRuleFinder = authorizationRuleFinder;
			_credentialContextService = credentialContextService;
		}

		/*public static string GetResourceString(string resourceKey)
		{
			Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceKey);
			StreamReader sr = new StreamReader(s);
			return sr.ReadToEnd();
		}*/

		const bool ConsoleOutput = false;
		const bool DatabaseExport = true;

		/// <summary>Executes sql create database scripts.</summary>
		public void Install()
		{
			MigrationManager.Migrate(GetConnectionString(), typeof(Migration001).Assembly, "Zeus.Installation.Migrations");
		}

		/*public void ExportSchema(TextWriter output)
		{
			SchemaExport exporter = new SchemaExport(Cfg);
			exporter.Execute(ConsoleOutput, DatabaseExport, false, true, null, output);
		}

		public void DropDatabaseTables()
		{
			SchemaExport exporter = new SchemaExport(Cfg);
			exporter.Drop(ConsoleOutput, DatabaseExport);
		}*/

		public DatabaseStatus GetStatus()
		{
			DatabaseStatus status = new DatabaseStatus();

			UpdateConnection(status);
			UpdateSchema(status);
			UpdateItems(status);

			return status;
		}

		public void CreateAdministratorUser(string username, string password)
		{
			UserCreateStatus createStatus;
			_credentialContextService.GetCurrentService().CreateUser(username, password, new[] { "Administrators" }, out createStatus);
			if (createStatus != UserCreateStatus.Success)
				throw new ZeusException("Could not create user: " + createStatus);
		}

		public string CreateDatabase(string server, string name)
		{
			return MigrationManager.CreateDatabase(server, name);
		}

		private void UpdateItems(DatabaseStatus status)
		{
			try
			{
				status.StartPageID = host.CurrentSite.StartPageID;
				status.RootItemID = host.CurrentSite.RootItemID;
				status.StartPage = persister.Get(status.StartPageID);
				status.RootItem = persister.Get(status.RootItemID);
				status.IsInstalled = status.RootItem != null && status.StartPage != null;

				status.HasUsers = _credentialContextService.GetCurrentService().GetUser("administrator") != null;
			}
			catch (Exception ex)
			{
				status.IsInstalled = false;
				status.ItemsError = ex.Message;
			}
		}

		private void UpdateSchema(DatabaseStatus status)
		{
			try
			{
				status.Items = _contentItemFinder.FindAll<ContentItem>().Count();
				status.Details = _contentDetailFinder.FindAll<PropertyData>().Count();
				status.DetailCollections = _detailCollectionFinder.FindAll<PropertyCollection>().Count();
				status.AuthorizedRoles = _authorizationRuleFinder.FindAll<AuthorizationRule>().Count();
				status.HasSchema = true;
			}
			catch (Exception ex)
			{
				status.HasSchema = false;
				status.SchemaError = ex.Message;
				status.SchemaException = ex;
			}
		}

		private void UpdateConnection(DatabaseStatus status)
		{
			try
			{
				using (IDbConnection conn = GetConnection())
				{
					conn.Open();
					conn.Close();
					status.ConnectionType = conn.GetType().Name;
				}
				status.IsConnected = true;
				status.ConnectionError = null;
			}
			catch (Exception ex)
			{
				status.IsConnected = false;
				status.ConnectionError = ex.Message;
				status.ConnectionException = ex;
			}
		}

		/// <summary>Method that will checks the database. If something goes wrong an exception is thrown.</summary>
		/// <returns>A string with diagnostic information about the database.</returns>
		public string CheckDatabase()
		{
			int itemCount = _contentItemFinder.FindAll<ContentItem>().Count();
			int detailCount = _contentDetailFinder.FindAll<PropertyData>().Count();
			int detailCollectionCount = _detailCollectionFinder.FindAll<PropertyCollection>().Count();
			int authorizationRuleCount = _authorizationRuleFinder.FindAll<AuthorizationRule>().Count();

			return string.Format("Database OK, items: {0}, details: {1}, authorization rules: {2}, detail collections: {3}",
													 itemCount, detailCount, authorizationRuleCount, detailCollectionCount);
		}

		/// <summary>Checks the root node in the database. Throws an exception if there is something really wrong with it.</summary>
		/// <returns>A diagnostic string about the root node.</returns>
		public string CheckRootItem()
		{
			int rootID = host.CurrentSite.RootItemID;
			ContentItem rootItem = persister.Get(rootID);
			if (rootItem != null)
				return string.Format("Root node OK, id: {0}, name: {1}, type: {2}, discriminator: {3}, published: {4} - {5}",
					rootItem.ID, rootItem.Name, rootItem.GetType(),
					_contentTypeManager.GetContentType(rootItem.GetType()), rootItem.Published, rootItem.Expires);
			return "No root item found with the id: " + rootID;
		}

		/// <summary>Checks the root node in the database. Throws an exception if there is something really wrong with it.</summary>
		/// <returns>A diagnostic string about the root node.</returns>
		public string CheckStartPage()
		{
			int startID = host.CurrentSite.StartPageID;
			ContentItem startPage = persister.Get(startID);
			if (startPage != null)
				return string.Format("Start page OK, id: {0}, name: {1}, type: {2}, discriminator: {3}, published: {4} - {5}",
					startPage.ID, startPage.Name, startPage.GetType(),
					_contentTypeManager.GetContentType(startPage.GetType()), startPage.Published, startPage.Expires);
			return "No start page found with the id: " + startID;
		}

		public ContentItem InsertRootNode(Type type, string name, string title)
		{
			ContentItem item = _contentTypeManager.CreateInstance(type, null);
			item.Name = name;
			item.Title = title;
			persister.Save(item);
			return item;
		}

		public ContentItem InsertStartPage(Type type, ContentItem root, string name, string title, string languageCode)
		{
			ContentItem item = _contentTypeManager.CreateInstance(type, root);
			item.Name = name;
			item.Title = title;
			item.Language = languageCode;
			persister.Save(item);
			return item;
		}

		#region Helper Methods

		public string GetConnectionString()
		{
			return ConfigurationManager.ConnectionStrings[GetConnectionStringName()].ConnectionString;
		}

		public string GetConnectionStringName()
		{
			DatabaseSection configSection = ConfigurationManager.GetSection("zeus/database") as DatabaseSection;
			if (configSection == null)
				throw new ZeusException("Missing <zeus/database> configuration section");
			ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[configSection.ConnectionStringName];
			if (connectionString == null)
				throw new ZeusException("Missing connection string '" + configSection.ConnectionStringName + "'");
			return configSection.ConnectionStringName;
		}

		public IDbConnection GetConnection()
		{
			return new SqlConnection(GetConnectionString());
		}

		/*public IDbConnection GetConnection()
		{
			IDriver driver = GetDriver();

			IDbConnection conn = driver.CreateConnection();
			if (Cfg.Properties.ContainsKey(Environment.ConnectionString))
				conn.ConnectionString = (string) Cfg.Properties[Environment.ConnectionString];
			else if (Cfg.Properties.ContainsKey(Environment.ConnectionStringName))
				conn.ConnectionString = ConfigurationManager.ConnectionStrings[(string) Cfg.Properties[Environment.ConnectionStringName]].ConnectionString;
			else
				throw new Exception("Didn't find a confgiured connection string or connection string name in the nhibernate configuration.");
			return conn;
		}

		public IDbCommand GenerateCommand(CommandType type, string sqlString)
		{
			IDriver driver = GetDriver();
			return driver.GenerateCommand(type, new NHibernate.SqlCommand.SqlString(sqlString), new SqlType[0]);
		}

		private IDriver GetDriver()
		{
			string driverName = (string) Cfg.Properties[Environment.ConnectionDriver];
			Type driverType = NHibernate.Util.ReflectHelper.ClassForName(driverName);
			return (IDriver) Activator.CreateInstance(driverType);
		}*/

		#endregion

		/*public ContentItem InsertExportFile(Stream stream, string filename)
		{
			IImportRecord record = importer.Read(stream, filename);
			importer.Import(record, null, ImportOption.All);

			return record.RootItem;
		}*/
	}
}
