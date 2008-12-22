using System;
using Zeus.Web.UI;
using Zeus;
using System.Web;
using System.Web.Security;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public abstract class SecurePage<T> : ContentPage<T>
		where T : ContentItem
	{
		public Web.Items.Customer Customer
		{
			get { return Zeus.Context.Persister.Get<Web.Items.Customer>((int) Membership.GetUser().ProviderUserKey); }
		}

		public Web.Items.ShoppingCart ShoppingCart
		{
			get
			{
				Web.Items.ShoppingCart shoppingCart = null;
				if (Request.Cookies["ShoppingCartID"] != null)
					shoppingCart = Zeus.Context.Persister.Get(Convert.ToInt32(Request.Cookies["ShoppingCartID"].Value)) as Web.Items.ShoppingCart;

				if (shoppingCart == null)
				{
					shoppingCart = new Web.Items.ShoppingCart();

					Web.Items.ShoppingCartContainer container = (Web.Items.ShoppingCartContainer) Find.RootItem.GetChild("ShoppingCarts");
					shoppingCart.AddTo(container);
					Zeus.Context.Persister.Save(shoppingCart);

					HttpCookie cookie = new HttpCookie("ShoppingCartID", shoppingCart.ID.ToString());
					cookie.Expires = DateTime.Now.AddYears(1);
					Response.Cookies.Add(cookie);
				}

				return shoppingCart;
			}
		}

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
	}
}
