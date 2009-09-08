using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.Admin;
using Zeus.Security;

namespace Zeus.AddIns.ECommerce.Plugins
{
	[ActionPluginGroup("ECommerce", 2000)]
	public class ECommerceActionPluginAttribute : ActionPluginAttribute
	{
		public ECommerceActionPluginAttribute(string name, string text, int sortOrder, string pageResourceName, string imageResourceName)
			: base(name, text, null, "ECommerce", sortOrder, null, pageResourceName, "selected={selected}", Targets.Preview, imageResourceName)
		{

		}

		public override ActionPluginState GetState(ContentItem contentItem, Zeus.Web.IWebContext webContext, ISecurityManager securityManager)
		{
			// Hide if this is not the OrderContainer node
			if (!(contentItem is OrderContainer))
				return ActionPluginState.Hidden;
			return base.GetState(contentItem, webContext, securityManager);
		}
	}
}