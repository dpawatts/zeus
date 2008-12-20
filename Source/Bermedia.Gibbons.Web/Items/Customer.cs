using System;

namespace Bermedia.Gibbons.Web.Items
{
	public class Customer : Zeus.Web.Security.Items.User
	{
		public string FirstName
		{
			get { return GetDetail<string>("FirstName", null); }
			set { SetDetail<string>("FirstName", value); }
		}

		public string LastName
		{
			get { return GetDetail<string>("LastName", null); }
			set { SetDetail<string>("LastName", value); }
		}

		public bool ReceiveOffers
		{
			get { return GetDetail<bool>("ReceiveOffers", false); }
			set { SetDetail<bool>("ReceiveOffers", value); }
		}
	}
}
