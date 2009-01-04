using System;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class Product : OnlineShopPage<Bermedia.Gibbons.Web.Items.StandardProduct>
	{
		protected void btnAddToCart_Click(object sender, EventArgs e)
		{
			Web.Items.ShoppingCartItem shoppingCartItem = new Web.Items.ShoppingCartItem();
			shoppingCartItem.Product = this.CurrentItem;
			shoppingCartItem.Quantity = Convert.ToInt32(txtQuantity.Text);

			Web.Items.ProductSizeLink size = null;
			if (!string.IsNullOrEmpty(ddlSizes.SelectedValue))
				size = Zeus.Context.Persister.Get<Web.Items.ProductSizeLink>(Convert.ToInt32(ddlSizes.SelectedValue));
			else if (shoppingCartItem.Product is Web.Items.FragranceBeautyProduct)
				size = ((Web.Items.FragranceBeautyProduct) shoppingCartItem.Product).Size;
			else if (shoppingCartItem.Product.AssociatedSizes.Count == 1)
				size = shoppingCartItem.Product.AssociatedSizes[0];
			shoppingCartItem.Size = size;

			if (!string.IsNullOrEmpty(ddlColours.SelectedValue))
				shoppingCartItem.Colour = (Web.Items.ProductColour) Zeus.Context.Persister.Get(Convert.ToInt32(ddlColours.SelectedValue));
			else if (!(shoppingCartItem.Product is Web.Items.FragranceBeautyProduct) && shoppingCartItem.Product.AssociatedColours.Count == 1)
				shoppingCartItem.Colour = (Web.Items.ProductColour) shoppingCartItem.Product.AssociatedColours[0];

			shoppingCartItem.AddTo(this.ShoppingCart);
			Zeus.Context.Persister.Save(shoppingCartItem);

			Response.Redirect(string.Format("~/shopping-cart.aspx?justadded={0}", this.CurrentItem.ID));
		}
	}
}
