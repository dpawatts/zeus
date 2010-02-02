using Ext.Net;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType]
	[RestrictParents(typeof(ShoppingBasket))]
	public class Address : BaseContentItem
	{
		public override string IconUrl
		{
			get { return GetIconUrl(Icon.EmailEdit); }
		}

		[ContentProperty("Title", 200)]
		public string PersonTitle
		{
			get { return GetDetail("PersonTitle", string.Empty); }
			set { SetDetail("PersonTitle", value); }
		}

		[ContentProperty("First Name", 210)]
		public string FirstName
		{
			get { return GetDetail("FirstName", string.Empty); }
			set { SetDetail("FirstName", value); }
		}

		[ContentProperty("Surname", 220)]
		public string Surname
		{
			get { return GetDetail("Surname", string.Empty); }
			set { SetDetail("Surname", value); }
		}

		[ContentProperty("Address Line One", 230)]
		public string AddressLine1
		{
			get { return GetDetail("AddressLine1", string.Empty); }
			set { SetDetail("AddressLine1", value); }
		}

		[ContentProperty("Address Line 2", 240)]
		public string AddressLine2
		{
			get { return GetDetail("AddressLine2", string.Empty); }
			set { SetDetail("AddressLine2", value); }
		}

		[ContentProperty("Town / City", 250)]
		public string TownCity
		{
			get { return GetDetail("TownCity", string.Empty); }
			set { SetDetail("TownCity", value); }
		}

		[ContentProperty("Postcode", 260)]
		public string Postcode
		{
			get { return GetDetail("Postcode", string.Empty); }
			set { SetDetail("Postcode", value); }
		}
	}
}