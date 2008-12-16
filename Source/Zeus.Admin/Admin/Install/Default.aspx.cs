using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Zeus.Installation;
using Zeus.ContentTypes;
using SoundInTheory.NMigration;
using System.Web.Hosting;
using System.IO;

namespace Zeus.Admin.Install
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			InstallationManager.Install();

			if (!IsPostBack)
			{
				ICollection<ContentType> definitions = Zeus.Context.Current.ContentTypes.GetContentTypes();
				foreach (ContentType definition in definitions)
					ddlRootItem.Items.Add(new ListItem(definition.ContentTypeAttribute.Title, definition.ItemType.AssemblyQualifiedName));
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			ContentItem item = Zeus.Context.Current.ContentTypes.CreateInstance(Type.GetType(ddlRootItem.SelectedValue), null);
			item.Name = "root";
			item.Title = "Root node";
			Zeus.Context.Persister.Save(item);
		}

		protected void btnInstallDynamicImageCaching_Click(object sender, EventArgs e)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["zeus"].ConnectionString;
			MigrationManager.Migrate(connectionString, Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"bin\SoundInTheory.DynamicImage.Caching.Linq.dll"), "SoundInTheory.DynamicImage.Caching.Migrations");
		}
	}
}
