using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "Address")]
	[RestrictParents(typeof(Customer))]
	public class Address : BaseContentItem
	{
		public string AddressLine1
		{
			get { return GetDetail<string>("AddressLine1", string.Empty); }
			set { SetDetail<string>("AddressLine1", value); }
		}

		public string AddressLine2
		{
			get { return GetDetail<string>("AddressLine2", string.Empty); }
			set { SetDetail<string>("AddressLine2", value); }
		}

		public string City
		{
			get { return GetDetail<string>("City", string.Empty); }
			set { SetDetail<string>("City", value); }
		}

		public string ParishState
		{
			get { return GetDetail<string>("ParishState", string.Empty); }
			set { SetDetail<string>("ParishState", value); }
		}

		public string Zip
		{
			get { return GetDetail<string>("Zip", string.Empty); }
			set { SetDetail<string>("Zip", value); }
		}

		public Country Country
		{
			get { return GetDetail<Country>("Country", null); }
			set { SetDetail<Country>("Country", value); }
		}

		public string PhoneNumber
		{
			get { return GetDetail<string>("PhoneNumber", string.Empty); }
			set { SetDetail<string>("PhoneNumber", value); }
		}

		protected override string IconName
		{
			get { return "ipod"; }
		}
	}
}
