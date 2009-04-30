using System.Linq;
using System.Web.UI.WebControls;
using Isis.Web.Hosting;
using Zeus.AddIns.Mailouts.Services;
using Zeus.Admin;
using Zeus.AddIns.Mailouts.ContentTypes;
using Zeus.Web;

[assembly: EmbeddedResourceFile("Zeus.AddIns.Mailouts.Admin.CampaignReport.aspx", "Zeus.AddIns.Mailouts.Admin")]
namespace Zeus.AddIns.Mailouts.Admin
{
	//[ReportActionPluginAttribute]
	public partial class CampaignReport : PreviewFrameAdminPage
	{
		public class ReportActionPluginAttribute : ActionPluginAttribute
		{
			public ReportActionPluginAttribute()
				: base("Report", "Report", null, "Mailout", 2, null, "Zeus.AddIns.Mailouts.Admin.CampaignReport.aspx", "selected={selected}", Targets.Preview, "Zeus.AddIns.Mailouts.Resources.report.png")
			{
				TypeFilter = typeof(Campaign);
			}

			protected override ActionPluginState GetStateInternal(ContentItem contentItem, IWebContext webContext)
			{
				return ((Campaign) contentItem).Status == CampaignStatus.Sent 
					? ActionPluginState.Enabled
					: ActionPluginState.Disabled;
			}
		}
	}
}
