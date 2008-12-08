using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Zeus.Installation;
using Zeus.ContentTypes;

namespace Zeus.Admin.Install
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			InstallationManager.Install();

			if (!IsPostBack)
			{
				ICollection<ContentType> definitions = Zeus.Context.Current.ContentTypes.GetDefinitions();
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
	}
}
