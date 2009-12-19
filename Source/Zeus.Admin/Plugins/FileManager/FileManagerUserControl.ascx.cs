using System;
using System.Collections.Generic;
using System.Linq;
using Coolite.Ext.Web;
using Zeus.Admin.Plugins.Tree;
using Zeus.Linq;
using Zeus.Security;
using Zeus.Web.Hosting;

namespace Zeus.Admin.Plugins.FileManager
{
	[AjaxMethodProxyID(Alias = "FileManager", IDMode = AjaxMethodProxyIDMode.Alias)]
	public partial class FileManagerUserControl : PluginUserControlBase
	{
		public FileManagerUserControl()
		{
			ID = "fileManager";
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			// Data loader.
			treePanel.Loader.Add(new Coolite.Ext.Web.TreeLoader
			{
				DataUrl =
					Engine.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(typeof (FileManagerUserControl).Assembly,
						"Zeus.Admin.Plugins.Tree.TreeLoader.ashx"),
				PreloadChildren = true
			});

			if (Ext.IsAjaxRequest)
				return;

			ContentItem uploadFolder = Find.StartPage.GetChild("Upload");
			if (uploadFolder == null)
				return;

			TreeNodeBase treeNode = SiteTree.Between(uploadFolder, uploadFolder, true)
				.OpenTo(uploadFolder)
				.Filter(items => items.Authorized(Engine.WebContext.User, Engine.SecurityManager, Operations.Read))
				.ToTreeNode(true);
			treePanel.Root.Add(treeNode);

			/*string path = Server.MapPath("../../Shared/images/thumbs");
			string[] files = System.IO.Directory.GetFiles(path);

			List<object> data = new List<object>(files.Length);
			foreach (string fileName in files)
			{
				System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
				data.Add(new
				{
					name = fi.Name,
					url = "../../Shared/images/thumbs/" + fi.Name,
					size = fi.Length,
					lastmod = fi.LastAccessTime
				});
			}

			filesStore.DataSource = data;
			filesStore.DataBind();*/
		}

		/*[AjaxMethod]
		public void DeleteItems(string ids)
		{
			if (string.IsNullOrEmpty(ids))
				return;

			string[] nodeIDsTemp = ids.Split(',');
			var nodeIDs = nodeIDsTemp.Select(s => Convert.ToInt32(s));
			if (!nodeIDs.Any())
				return;

			ContentItem parent = Engine.Persister.Get(nodeIDs.First()).Parent;
			foreach (int id in nodeIDs)
			{
				ContentItem item = Engine.Persister.Get(id);
				Zeus.Context.Persister.Delete(item);
			}

			if (parent != null)
				Refresh(parent);
			else
				Refresh(Zeus.Context.UrlParser.StartPage);
		}*/
	}
}