using Isis.Web.UI;
using Zeus.ContentTypes;
using Zeus.Integrity;

namespace Zeus.Web.Security.Items
{
	[ContentType("Security Container")]
	[RestrictParents(typeof(IRootItem))]
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
			get { return WebResourceUtility.GetUrl(typeof(SecurityContainer), "Zeus.Web.Resources.Icons.folder_key.png"); }
		}
	}
}
