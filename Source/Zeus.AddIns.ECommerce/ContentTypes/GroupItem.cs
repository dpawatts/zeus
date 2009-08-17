using Zeus.FileSystem.Images;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(Group), typeof(GroupItem))]
	public class GroupItem : BaseContentItem
	{
		[ContentProperty("Description", 100)]
		public virtual string Description
		{
			get { return GetDetail("Description", string.Empty); }
			set { SetDetail("Description", value); }
		}

		[ContentProperty("Image", 110)]
		public virtual ImageData Image
		{
			get { return GetDetail<ImageData>("Image", null); }
			set { SetDetail("Image", value); }
		}
	}
}