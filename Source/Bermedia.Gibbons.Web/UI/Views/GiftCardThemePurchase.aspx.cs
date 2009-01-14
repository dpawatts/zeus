using System;
using Isis.ExtensionMethods.Web;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class GiftCardThemePurchase : OnlineShopPage<Bermedia.Gibbons.Web.Items.GiftCardThemePurchasePage>
	{
		protected void btnBuy_Click(object sender, EventArgs e)
		{
			if (!IsValid)
				return;

			Web.Items.GiftCardShoppingCartItem shoppingCartItem = new Web.Items.GiftCardShoppingCartItem();
			shoppingCartItem.RecipientName = txtName.Text;
			shoppingCartItem.PricePerUnit = Convert.ToDecimal(txtAmount.Text);
			shoppingCartItem.Quantity = 1;
			shoppingCartItem.Image = Zeus.Context.Persister.Get<Web.Items.GiftCardTheme>(Request.GetRequiredInt("id")).Image;

			shoppingCartItem.AddTo(this.ShoppingCart);
			Zeus.Context.Persister.Save(shoppingCartItem);

			Response.Redirect("~/shopping-cart.aspx");
		}
	}
}
