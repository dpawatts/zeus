using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web.UI;
using Zeus.Configuration;
using Zeus.ContentTypes;
using Zeus.Installation;
using Zeus.Web.UI;

namespace Zeus.Admin.Install
{
	public partial class Default : System.Web.UI.Page
	{
		protected CustomValidator cvExisting;
		protected Label errorLabel;
		//protected FileUpload fileUpload;

		private DatabaseStatus status;

		protected int RootId
		{
			get { return (int) (ViewState["rootId"] ?? 0); }
			set { ViewState["rootId"] = value; }
		}
		protected int StartId
		{
			get { return (int) (ViewState["startId"] ?? 0); }
			set { ViewState["startId"] = value; }
		}

		//protected RadioButtonList rblExports;

		public InstallationManager CurrentInstallationManager
		{
			get { return Zeus.Context.Current.Resolve<InstallationManager>(); }
		}

		public DatabaseStatus Status
		{
			get
			{
				if (status == null)
					status = CurrentInstallationManager.GetStatus();
				return status;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			try
			{
				ICollection<ContentType> preferredRoots = new List<ContentType>();
				ICollection<ContentType> preferredStartPages = new List<ContentType>();
				ICollection<ContentType> preferredRootAndStartPages = new List<ContentType>();

				ICollection<ContentType> fallbackRoots = new List<ContentType>();
				ICollection<ContentType> fallbackStartPages = new List<ContentType>();

				foreach (ContentType d in Zeus.Context.ContentTypes.GetContentTypes())
				{
					InstallerHints hint = d.ContentTypeAttribute.Installer;

					if (Is(hint, InstallerHints.PreferredRootPage))
						preferredRoots.Add(d);
					if (Is(hint, InstallerHints.PreferredStartPage))
						preferredStartPages.Add(d);
					if (Is(hint, InstallerHints.PreferredRootPage) || Is(hint, InstallerHints.PreferredStartPage))
						preferredRootAndStartPages.Add(d);
					if (!Is(hint, InstallerHints.NeverRootPage))
						fallbackRoots.Add(d);
					if (!Is(hint, InstallerHints.NeverStartPage))
						fallbackStartPages.Add(d);
				}

				if (preferredRoots.Count == 0)
					preferredRoots = fallbackRoots;
				if (preferredStartPages.Count == 0)
					preferredStartPages = fallbackStartPages;

				LoadRootTypes(ddlRoot, preferredRoots, "[root node]");
				LoadStartTypes(ddlStartPage, preferredStartPages, "[start node]");
				LoadRootTypes(ddlRootAndStart, preferredRootAndStartPages, "[root and start node]");
				//LoadExistingExports();
			}
			catch (Exception ex)
			{
				ltStartupError.Text = "<li style='color:red'>Ooops, something is wrong: " + ex.Message + "</li>";
				return;
			}

			base.OnInit(e);
		}

		protected override void OnError(EventArgs e)
		{
			errorLabel.Text = FormatException(Server.GetLastError());
		}

		/*private void LoadExistingExports()
		{
			string dir = HostingEnvironment.MapPath("~/App_Data");
			if (Directory.Exists(dir))
			{
				foreach (string file in Directory.GetFiles(dir, "*.gz"))
				{
					rblExports.Items.Add(new ListItem(Path.GetFileName(file)));
				}
			}

			btnInsertExport.Enabled = rblExports.Items.Count > 0;
		}*/

		private static bool Is(InstallerHints flags, InstallerHints expected)
		{
			return (flags & expected) == expected;
		}

		private void LoadStartTypes(ListControl lc, IEnumerable<ContentType> startPageDefinitions, string initialText)
		{
			lc.Items.Clear();
			lc.Items.Add(initialText);
			foreach (ContentType d in startPageDefinitions)
				lc.Items.Add(new ListItem(d.Title, d.ItemType.AssemblyQualifiedName));
		}

		private static void LoadRootTypes(ListControl lc, IEnumerable<ContentType> rootDefinitions, string initialText)
		{
			lc.Items.Clear();
			lc.Items.Add(initialText);
			foreach (ContentType d in rootDefinitions)
				lc.Items.Add(new ListItem(d.Title, d.ItemType.AssemblyQualifiedName));
		}

		protected void btnTest_Click(object sender, EventArgs e)
		{
			try
			{
				InstallationManager im = CurrentInstallationManager;

				using (IDbConnection conn = im.GetConnection())
				{
					conn.Open();
					lblStatus.CssClass = "ok";
					lblStatus.Text = "Connection OK";
				}
			}
			catch (Exception ex)
			{
				lblStatus.CssClass = "warning";
				lblStatus.Text = "Connection problem, hopefully this error message can help you figure out what's wrong: <br/>" +
												 ex.Message;
				lblStatus.ToolTip = ex.StackTrace;
			}
		}

		protected void btnInstall_Click(object sender, EventArgs e)
		{
			InstallationManager im = CurrentInstallationManager;
			if (ExecuteWithErrorHandling(im.Install) != null)
				if (ExecuteWithErrorHandling(im.Install) == null)
					lblInstall.Text = "Database created, now insert root items.";
		}

		/*protected void btnExportSchema_Click(object sender, EventArgs e)
		{
			Response.ContentType = "application/octet-stream";
			Response.AddHeader("Content-Disposition", "attachment;filename=zeus.sql");

			InstallationManager im = CurrentInstallationManager;
			im.ExportSchema(Response.Output);

			Response.End();
		}*/

		protected void btnInsert_Click(object sender, EventArgs e)
		{
			InstallationManager im = CurrentInstallationManager;

			try
			{
				cvRootAndStart.IsValid = ddlRoot.SelectedIndex > 0 && ddlStartPage.SelectedIndex > 0;
				cvRoot.IsValid = true;
				if (!cvRootAndStart.IsValid)
					return;

				ContentItem root = im.InsertRootNode(Type.GetType(ddlRoot.SelectedValue), "root", "Root Node");
				ContentItem startPage = im.InsertStartPage(Type.GetType(ddlStartPage.SelectedValue), root, "start", "Start Page", Zeus.Context.Current.LanguageManager.GetDefaultLanguage());

				if (startPage.ID == Status.StartPageID && root.ID == Status.RootItemID)
				{
					ltRootNode.Text = "<span class='ok'>Root and start pages inserted.</span>";
				}
				else
				{
					ltRootNode.Text = string.Format(
						"<span class='warning'>Start page inserted but you must update web.config with root item id: <b>{0}</b> and start page id: <b>{1}</b></span>", root.ID, startPage.ID);
					phSame.Visible = false;
					phDiffer.Visible = true;
					RootId = root.ID;
					StartId = startPage.ID;
				}
				phDiffer.DataBind();
			}
			catch (Exception ex)
			{
				ltRootNode.Text = string.Format("<span class='warning'>{0}</span><!--\n{1}\n-->", ex.Message, ex);
			}
		}
		protected void btnInsertRootOnly_Click(object sender, EventArgs e)
		{
			InstallationManager im = CurrentInstallationManager;

			try
			{
				cvRootAndStart.IsValid = true;
				cvRoot.IsValid = ddlRootAndStart.SelectedIndex > 0;
				if (!cvRoot.IsValid)
					return;

				ContentItem root = im.InsertRootNode(Type.GetType(ddlRootAndStart.SelectedValue), "start", "Start Page");

				if (root.ID == Status.RootItemID && root.ID == Status.StartPageID)
				{
					ltRootNode.Text = "<span class='ok'>Root node inserted.</span>";
					phSame.Visible = false;
					phDiffer.Visible = false;
					RootId = root.ID;
				}
				else
				{
					ltRootNode.Text = string.Format(
						"<span class='warning'>Root node inserted but you must update web.config with root item id: <b>{0}</b></span> ",
						root.ID);
					phSame.Visible = true;
					phDiffer.Visible = false;
					RootId = root.ID;
					StartId = root.ID;
				}
				phSame.DataBind();
			}
			catch (Exception ex)
			{
				ltRootNode.Text = string.Format("<span class='warning'>{0}</span><!--\n{1}\n-->", ex.Message, ex);
			}
		}

		/*protected void btnInsertExport_Click(object sender, EventArgs e)
		{
			cvExisting.IsValid = rblExports.SelectedIndex >= 0;
			if (!cvExisting.IsValid)
				return;

			string path = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), rblExports.SelectedValue);
			ExecuteWithErrorHandling(delegate { InsertFromFile(path); });
		}*/

		/*private void InsertFromFile(string path)
		{
			InstallationManager im = CurrentInstallationManager;
			using (Stream read = File.OpenRead(path))
			{
				ContentItem root = im.InsertExportFile(read, path);
				InsertRoot(root);
			}
		}*/

		/*protected void btnUpload_Click(object sender, EventArgs e)
		{
			rfvUpload.IsValid = fileUpload.PostedFile != null && fileUpload.PostedFile.FileName.Length > 0;
			if (!rfvUpload.IsValid)
				return;

			ExecuteWithErrorHandling(InstallFromUpload);
		}*/

		protected void btnUpdateWebConfig_Click(object sender, EventArgs e)
		{
			if (ExecuteWithErrorHandling(SaveConfiguration) == null)
				lblWebConfigUpdated.Text = "Configuration updated.";
		}

		private void SaveConfiguration()
		{
			System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration("~");

			HostSection host = (HostSection) cfg.GetSection("zeus/host");
			host.RootItemID = RootId;

			host.Sites.Clear();

			SiteElement site = new SiteElement
     	{
     		ID = "DefaultSite",
     		Description = "Default Site",
     		StartPageID = StartId
     	};
			site.SiteHosts.Add(new HostNameElement { Name = "*" });
			host.Sites.Add(site);

			cfg.Save();
		}

		protected void btnRestart_Click(object sender, EventArgs e)
		{
			HttpRuntime.UnloadAppDomain();
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.reset.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.shared.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.preview.css");

			Page.ClientScript.RegisterJQuery();

			DataBind();

			base.OnPreRender(e);
		}

		protected string GetStatusText()
		{
			if (Status.IsInstalled)
				return "You're all set (just check step 6).";
			if (Status.HasSchema)
				return "Jump to step 4.";
			if (Status.IsConnected)
				return "Skip to step 3.";
			return "Continue to step 2.";
		}

		/*private void InstallFromUpload()
		{
			InstallationManager im = CurrentInstallationManager;
			ContentItem root = im.InsertExportFile(fileUpload.FileContent, fileUpload.FileName);

			InsertRoot(root);
		}*/

		private void InsertRoot(ContentItem root)
		{
			if (root.ID == Status.RootItemID)
			{
				ltRootNode.Text = "<span class='ok'>Root node inserted.</span>";
				phSame.Visible = false;
				phDiffer.Visible = false;
			}
			else
			{
				ltRootNode.Text = string.Format(
					"<span class='warning'>Root node inserted but you must update web.config with root item id: <b>{0}</b></span> ",
					root.ID);
				phSame.Visible = true;
				phDiffer.Visible = false;
				RootId = root.ID;
				StartId = root.ID;
				phSame.DataBind();
			}

			// try to find a suitable start page
			foreach (ContentItem item in root.Children)
			{
				ContentType id = Zeus.Context.ContentTypes.GetContentType(item.GetType());
				if (Is(id.ContentTypeAttribute.Installer, InstallerHints.PreferredStartPage))
				{
					if (item.ID == Status.StartPageID && root.ID == Status.RootItemID)
					{
						ltRootNode.Text = "<span class='ok'>Root and start page inserted.</span>";
					}
					else
					{
						ltRootNode.Text = string.Format(
							"<span class='warning'>Start page inserted but you must update web.config with root item id: <b>{0}</b> and start page id: <b>{1}</b></span>", root.ID, item.ID);
						phSame.Visible = false;
						phDiffer.Visible = true;
						StartId = item.ID;
						RootId = root.ID;
					}
					break;
				}
				phDiffer.DataBind();
			}
		}

		public delegate void Execute();

		protected Exception ExecuteWithErrorHandling(Execute action)
		{
			return ExecuteWithErrorHandling<Exception>(action);
		}

		protected T ExecuteWithErrorHandling<T>(Execute action)
			where T : Exception
		{
			try
			{
				action();
				return null;
			}
			catch (T ex)
			{
				errorLabel.Text = FormatException(ex);
				return ex;
			}
		}

		private static string FormatException(Exception ex)
		{
			if (ex == null)
				return "Unknown error";
			return "<b>" + ex.Message + "</b>" + ex.StackTrace;
		}

		protected void btnCreateAdministrator_Click(object sender, EventArgs e)
		{
			// Attempt to create user.
			try
			{
				CurrentInstallationManager.CreateAdministratorUser(txtAdministratorUsername.Text, txtAdministratorPassword.Text);
				ltlAdminLogin.Text = "<span class='ok'>Administrator login created.</span>";
			}
			catch (Exception ex)
			{
				ltlAdminLogin.Text = string.Format("<span class='warning'>{0}</span><!--\n{1}\n-->", ex.ToString(), ex);
			}
		}

		protected void btnCreateDatabase_Click(object sender, EventArgs e)
		{
			// Attempt to create user.
			try
			{
				string connectionString = CurrentInstallationManager.CreateDatabase(txtDatabaseServer.Text, txtDatabaseName.Text);

				System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration("~");
				ConnectionStringSettings connectionStringSettings = cfg.ConnectionStrings.ConnectionStrings[CurrentInstallationManager.GetConnectionStringName()];
				connectionStringSettings.ConnectionString = connectionString;
				cfg.Save();

				ltlCreateDatabase.Text = "<span class='ok'>Database created and web.config updated.</span>";
			}
			catch (Exception ex)
			{
				ltlCreateDatabase.Text = string.Format("<span class='warning'>{0}</span><!--\n{1}\n-->", ex.Message, ex);
			}
		}

		protected void btnTurnOffInstallationMode_Click(object sender, EventArgs e)
		{
			try
			{
				System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration("~");
				AdminSection adminConfig = (AdminSection) cfg.GetSection("zeus/admin");
				adminConfig.Installer.Mode = InstallationMode.Normal;
				cfg.Save();

				ltlInstallationMode.Text = "<span class='ok'>Installation mode turned off in web.config.</span>";
			}
			catch (Exception ex)
			{
				ltlInstallationMode.Text = string.Format("<span class='warning'>{0}</span><!--\n{1}\n-->", ex.Message, ex);
			}
		}

		protected void btnUpload_Click(object sender, EventArgs e)
		{
			rfvUpload.IsValid = fileUpload.PostedFile != null && fileUpload.PostedFile.FileName.Length > 0;
			if (!rfvUpload.IsValid)
				return;

			ExecuteWithErrorHandling(InstallFromUpload);
		}

		private void InstallFromUpload()
		{
			ContentItem root = CurrentInstallationManager.InsertExportFile(fileUpload.FileContent, fileUpload.FileName);
			InsertRoot(root);
		}
	}
}
