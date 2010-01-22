using System;
using Ext.Net;
using Zeus.Web.Caching;

namespace Zeus.Admin.Plugins.PageCaching
{
	[DirectMethodProxyID(Alias = "PageCaching", IDMode = DirectMethodProxyIDMode.Alias)]
	public partial class PageCachingUserControl : PluginUserControlBase
	{
		[DirectMethod]
		public void DeleteCachedPage(string id)
		{
			if (string.IsNullOrEmpty(id))
				return;

			ContentItem contentItem = Engine.Persister.Get(Convert.ToInt32(id));
			Engine.Resolve<ICachingService>().DeleteCachedPage(contentItem);
		}
	}
}