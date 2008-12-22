using System;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	public class Customer : Zeus.Web.Security.Items.User
	{
		public string FirstName
		{
			get { return GetDetail<string>("FirstName", string.Empty); }
			set { SetDetail<string>("FirstName", value); }
		}

		public string LastName
		{
			get { return GetDetail<string>("LastName", string.Empty); }
			set { SetDetail<string>("LastName", value); }
		}

		public string FullName
		{
			get { return (this.FirstName + " " + this.LastName).Trim(); }
		}

		public bool ReceiveOffers
		{
			get { return GetDetail<bool>("ReceiveOffers", false); }
			set { SetDetail<bool>("ReceiveOffers", value); }
		}

		public DetailCollection ShippingAddresses
		{
			get { return GetDetailCollection("ShippingAddresses", true); }
		}

		public DetailCollection BillingAddresses
		{
			get { return GetDetailCollection("BillingAddresses", true); }
		}
	}
}
