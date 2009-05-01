using Zeus.Configuration;
using Zeus.Security;

namespace Zeus.Admin.Versions
{
	public class VersioningActionPluginAttribute : ActionPluginAttribute
	{
		public VersioningActionPluginAttribute(string name, string text, int sortOrder, string imageResourceName)
			: base(name, text, Operations.Version, "ViewPreview", sortOrder, null, "Zeus.Admin.Versions.Default.aspx", "selected={selected}", Targets.Preview, imageResourceName)
		{

		}

		public override ActionPluginState GetState(ContentItem contentItem, Zeus.Web.IWebContext webContext, ISecurityManager securityManager)
		{
			// Hide, if versioning is disabled.
			if (!Context.Current.Resolve<AdminSection>().Versioning.Enabled)
				return ActionPluginState.Hidden;
			return base.GetState(contentItem, webContext, securityManager);
		}
	}
}