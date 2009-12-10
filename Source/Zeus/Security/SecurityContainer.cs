using Coolite.Ext.Web;
using Zeus.BaseLibrary.Web.UI;
using Zeus.Integrity;

namespace Zeus.Security.ContentTypes
{
	[ContentType("Security Container")]
	[RestrictParents(typeof(SystemNode))]
	public class SecurityContainer : ContentItem
	{
		public const string ContainerName = "security";
		public const string ContainerTitle = "Security";

		public SecurityContainer()
		{
			Name = ContainerName;
			Title = ContainerTitle;
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.FolderKey); }
		}
	}
}
