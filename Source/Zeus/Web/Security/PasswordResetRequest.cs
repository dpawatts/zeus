using Zeus.Integrity;
using Zeus.Security;

namespace Zeus.Web.Security
{
	[ContentType("Password Reset Request")]
	[RestrictParents(typeof(User))]
	public class PasswordResetRequest : DataContentItem
	{
		public PasswordResetRequest()
		{
			Title = "Password Reset Request";
		}

		[ContentProperty("Nonce", 100)]
		public virtual string Nonce
		{
			get { return GetDetail("Nonce", string.Empty); }
			set { SetDetail("Nonce", value); }
		}

		[ContentProperty("Used", 110)]
		public virtual bool Used
		{
			get { return GetDetail("Used", false); }
			set { SetDetail("Used", value); }
		}
	}
}