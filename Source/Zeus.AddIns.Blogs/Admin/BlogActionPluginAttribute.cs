using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Admin;
using Zeus.Security;

namespace Zeus.AddIns.Blogs.Admin
{
	[ActionPluginGroup("Blog", 2000)]
	public class BlogActionPluginAttribute : ActionPluginAttribute
	{
		public BlogActionPluginAttribute(string name, string text, int sortOrder, string pageResourceName, string imageResourceName)
			: base(name, text, null, "Blog", sortOrder, null, pageResourceName, "selected={selected}", Targets.Preview, imageResourceName)
		{

		}

		public override ActionPluginState GetState(ContentItem contentItem, Zeus.Web.IWebContext webContext, ISecurityManager securityManager)
		{
			// Hide if this is not the OrderContainer node
			if (!(contentItem is Blog))
				return ActionPluginState.Hidden;
			return base.GetState(contentItem, webContext, securityManager);
		}
	}
}