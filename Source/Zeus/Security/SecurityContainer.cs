using Ext.Net;
using Zeus.Integrity;

namespace Zeus.Security.ContentTypes
{
	[ContentType("Security Container")]
	[RestrictParents(typeof(SystemNode))]
	public class SecurityContainer : DataContentItem
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
