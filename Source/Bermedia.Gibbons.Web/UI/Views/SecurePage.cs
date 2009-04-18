using System;
using Zeus.Web.UI;
using Zeus;
using System.Web;
using System.Web.Security;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public abstract class SecurePage<T> : OnlineShopPage<T>
		where T : ContentItem
	{
		public CheckoutData CheckoutData
		{
			get
			{
				CheckoutData checkoutData = Session["CheckoutData"] as CheckoutData;
				if (checkoutData == null)
				{
					checkoutData = new CheckoutData();
					Session["CheckoutData"] = checkoutData;
				}
				return checkoutData;
			}
		}

		protected void ClearCheckoutData()
		{
			Session["CheckoutData"] = null;
		}

		public Web.Items.Customer Customer
		{
			get
			{
				MembershipUser membershipUser = Membership.GetUser();
				if (membershipUser != null)
					return Zeus.Context.Persister.Get<Items.Customer>((int) membershipUser.ProviderUserKey);
				else
				{
					Response.Redirect("/");
					return null;
				}
			}
		}
	}
}
