using Zeus.Integrity;

namespace Zeus.AddIns.Mailouts.ContentTypes
{
	[ContentType]
	[RestrictParents(AllowedTypes.None)]
	public class RecipientBounce : ContentItem
	{
		public virtual string Email
		{
			get { return GetDetail("Email", string.Empty); }
			set { SetDetail("Email", value); }
		}

		public virtual string Error
		{
			get { return GetDetail("Error", string.Empty); }
			set { SetDetail("Error", value); }
		}
	}
}
