using Zeus.Configuration;
using Zeus.Security;

namespace Zeus.Admin.ImportExport
{
	public class ImportExportActionPluginAttribute : ActionPluginAttribute
	{
		public ImportExportActionPluginAttribute(string name, string text, int sortOrder, string imageResourceName)
			: base(name, text, Operations.ImportExport, "ViewPreview", sortOrder, null, "Zeus.Admin.ImportExport.Default.aspx", "selected={selected}", Targets.Preview, imageResourceName)
		{

		}

		public override ActionPluginState GetState(ContentItem contentItem, Zeus.Web.IWebContext webContext, ISecurityManager securityManager)
		{
			// Hide, if import / export is disabled.
			if (!Context.Current.Resolve<AdminSection>().ImportExportEnabled)
				return ActionPluginState.Hidden;
			return base.GetState(contentItem, webContext, securityManager);
		}
	}
}