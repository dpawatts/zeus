using Zeus.Configuration;
using Zeus.Security;

namespace Zeus.Admin.Globalization
{
	public class LanguageActionPluginAttribute : ActionPluginAttribute
	{
		public LanguageActionPluginAttribute(string name, string text, int sortOrder, string pageResourcePath, string imageResourceName)
			: base(name, text, Operations.Administer, "Globalization", sortOrder, null, pageResourcePath, "selected={selected}", Targets.Preview, imageResourceName)
		{
		}

		public override ActionPluginState GetState(ContentItem contentItem, Zeus.Web.IWebContext webContext, ISecurityManager securityManager)
		{
			// Hide, if globalization is disabled.
			if (!Context.Current.Resolve<GlobalizationSection>().Enabled)
				return ActionPluginState.Hidden;
			return base.GetState(contentItem, webContext, securityManager);
		}
	}
}