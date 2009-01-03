using System;
using System.Linq;
using Zeus.ContentTypes.Properties;
using System.Collections.Generic;

namespace Bermedia.Gibbons.Web.Items
{
	public class Customer : Zeus.Web.Security.Items.User
	{
		public override string Email
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

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

		public NewsletterSubscription NewsletterSubscription
		{
			get { return GetDetail<NewsletterSubscription>("NewsletterSubscription", null); }
			set { SetDetail<NewsletterSubscription>("NewsletterSubscription", value); }
		}

		public string FullName
		{
			get { return (this.FirstName + " " + this.LastName).Trim(); }
		}

		public DetailCollection ShippingAddresses
		{
			get { return GetDetailCollection("ShippingAddresses", true); }
		}

		public DetailCollection BillingAddresses
		{
			get { return GetDetailCollection("BillingAddresses", true); }
		}

		public IEnumerable<Order> Orders
		{
			get { return this.GetChildren<Order>().Where(o => o.Status != OrderStatus.Basket); }
		}
	}
}
