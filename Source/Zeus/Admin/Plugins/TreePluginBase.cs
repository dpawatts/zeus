using System;
using Ext.Net;
using Zeus.Web.Hosting;

namespace Zeus.Admin.Plugins
{
	public abstract class TreePluginBase : ITreePlugin
	{
		public virtual string[] RequiredScripts
		{
			get { return null; }
		}

		public virtual string[] RequiredUserControls
		{
			get { return null; }
		}

		public abstract void ModifyTree(TreePanel treePanel, IMainInterface mainInterface);

		public virtual void ModifyTreeNode(TreeNodeBase treeNode, ContentItem contentItem)
		{
			
		}

		protected string GetPageUrl(Type type, string resourcePath)
		{
			return Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(type.Assembly, resourcePath);
		}
	}
}