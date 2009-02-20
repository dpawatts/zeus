using System;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class Product : OnlineShopPage<Items.StandardProduct>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			rfvSizes.Enabled = ddlSizes.Visible;
			rfvColours.Enabled = ddlColours.Visible;

			if (!IsPostBack)
			{
				decimal? salePrice; decimal regularPrice;
				if (CurrentItem.AssociatedSizes.Count == 1 && !(CurrentItem is Items.FragranceBeautyProduct) && CurrentItem.AssociatedSizes[0].CurrentPrice != null)
				{
					salePrice = CurrentItem.AssociatedSizes[0].SalePrice;
					regularPrice = CurrentItem.AssociatedSizes[0].RegularPrice.Value;
				}
				else
				{
					salePrice = CurrentItem.SalePrice;
					regularPrice = CurrentItem.RegularPrice;
				}
				UpdatePrice(salePrice, regularPrice);
			}
		}

		private void UpdatePrice(decimal? salePrice, decimal? regularPrice)
		{
			plcRegularPriceOnly.Visible = plcSalePrice.Visible = false;

			decimal newRegularPrice = regularPrice ?? CurrentItem.RegularPrice;
			decimal? newSalePrice = salePrice ?? ((regularPrice != null) ? null : CurrentItem.SalePrice);
			if (newSalePrice != null)
			{
				ltlSalePrice.Text = newSalePrice.Value.ToString("C2");
				ltlOldPrice.Text = newRegularPrice.ToString("C2");
				plcSalePrice.Visible = true;
			}
			else
			{
				h2Price.InnerText = newRegularPrice.ToString("C2");
				plcRegularPriceOnly.Visible = true;
			}
		}

		protected void ddlSizes_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(ddlSizes.SelectedValue))
			{
				Items.ProductSizeLink productSizeLink = Zeus.Context.Persister.Get<Items.ProductSizeLink>(Convert.ToInt32(ddlSizes.SelectedValue));
				UpdatePrice(productSizeLink.SalePrice, productSizeLink.RegularPrice);
			}
			else
			{
				UpdatePrice(CurrentItem.SalePrice, CurrentItem.RegularPrice);
			}
		}

		protected void btnAddToCart_Click(object sender, EventArgs e)
		{
			if (!IsValid)
				return;

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
