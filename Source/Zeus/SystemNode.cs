using Coolite.Ext.Web;
using Zeus.Integrity;

namespace Zeus
{
	[ContentType("System Node")]
	[RestrictParents(typeof(RootItem))]
	public class SystemNode : DataContentItem
	{
		public SystemNode()
		{
			Name = "system";
			Title = "System";
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Computer); }
		}
	}
}
