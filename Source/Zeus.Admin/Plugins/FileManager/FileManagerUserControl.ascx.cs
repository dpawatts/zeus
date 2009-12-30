using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Coolite.Ext.Web;
using Zeus.Admin.Plugins.Tree;
using Zeus.FileSystem;
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
			var treeloader = new Coolite.Ext.Web.TreeLoader
				{
					DataUrl =
						Engine.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(typeof(FileManagerUserControl).Assembly,
							"Zeus.Admin.Plugins.FileManager.TreeLoader.ashx"),
					PreloadChildren = true
				};
			treePanel.Loader.Add(treeloader);

			if (Ext.IsAjaxRequest)
				return;

			ContentItem fileManagerRootFolder = Find.StartPage;
			if (fileManagerRootFolder == null)
				return;

			TreeNodeBase treeNode = SiteTree.Between(fileManagerRootFolder, fileManagerRootFolder, true)
				.OpenTo(fileManagerRootFolder)
				.Filter(items => items.Authorized(Engine.WebContext.User, Engine.SecurityManager, Operations.Read))
				.ToTreeNode(true);
			treePanel.Root.Add(treeNode);

			filesStore.DataSource = GetFiles(fileManagerRootFolder, FileType.Both);
			filesStore.DataBind();
		}

		private static IEnumerable GetFiles(ContentItem contentItem, FileType fileType)
		{
			IEnumerable<ContentItem> files = contentItem.GetChildren().Navigable();
			switch (fileType)
			{
				case FileType.Image :
					files = files.Where(f => f is FileSystem.Images.Image);
					break;
			}
			return files.Select(ci => new
				{
					name = ci.Title,
					url = GetUrl(fileType, ci),
					imageUrl = GetImageUrl(ci)
				});
		}

		private static string GetUrl(FileType fileType, ContentItem ci)
		{
			if (fileType == FileType.Image)
				return ci.Url;
			return "~/link/" + ci.ID;
		}

		private static string GetImageUrl(ContentItem ci)
		{
			if (ci is FileSystem.Images.Image)
				return ((FileSystem.Images.Image) ci).GetUrl(80, 60, true);
			return ci.IconUrl;
		}

		protected void filesStore_RefreshData(object sender, StoreRefreshDataEventArgs e)
		{
			int nodeID = Convert.ToInt32(e.Parameters["node"]);
			FileType type = e.Parameters.Any(p => p.Name == "type") ? (FileType) Enum.Parse(typeof(FileType), e.Parameters["type"], true) : FileType.Both;
			ContentItem contentItem = Engine.Persister.Get(nodeID);
			filesStore.DataSource = GetFiles(contentItem, type);
			filesStore.DataBind();
		}

		private enum FileType
		{
			File,
			Image,
			Both
		}
	}
}