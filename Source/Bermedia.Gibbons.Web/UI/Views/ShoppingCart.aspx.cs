using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.Web;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class ShoppingCart : OnlineShopPage<Web.Items.ShoppingCartPage>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				if (Request.QueryString["add"] != null)
				{
					Web.Items.ShoppingCartItem shoppingCartItem = new Web.Items.ShoppingCartItem();
					shoppingCartItem.Product = (Web.Items.StandardProduct) Zeus.Context.Persister.Get(Request.GetRequiredInt("add"));
					shoppingCartItem.Quantity = Request.GetRequiredInt("quantity");

					Web.Items.ProductSizeLink size = null;
					if (Request.QueryString["size"] != null)
						size = Zeus.Context.Persister.Get<Web.Items.ProductSizeLink>(Request.GetRequiredInt("size"));
					else if (shoppingCartItem.Product is Web.Items.FragranceBeautyProduct)
						size = ((Web.Items.FragranceBeautyProduct)shoppingCartItem.Product).Size;
					else if (shoppingCartItem.Product.AssociatedSizes.Count == 1)
						size = shoppingCartItem.Product.AssociatedSizes[0];
					shoppingCartItem.Size = size;

					if (Request.QueryString["colour"] != null)
						shoppingCartItem.Colour = (Web.Items.ProductColour) Zeus.Context.Persister.Get(Request.GetRequiredInt("colour"));
					else if (!(shoppingCartItem.Product is Web.Items.FragranceBeautyProduct) && shoppingCartItem.Product.AssociatedColours.Count == 1)
						shoppingCartItem.Colour = (Web.Items.ProductColour) shoppingCartItem.Product.AssociatedColours[0];

					shoppingCartItem.AddTo(this.ShoppingCart);
					Zeus.Context.Persister.Save(shoppingCartItem);

					Response.Redirect("~/shopping-cart.aspx");
				}

				ReBind();
			}
		}

		private void ReBind()
		{
			// Get ShoppingCart for current user.
			lsvShoppingCartItems.DataSource = this.ShoppingCart.GetChildren();
			lsvShoppingCartItems.DataBind();
		}

		protected void btnUpdateQuantities_Click(object sender, EventArgs e)
		{
			List<ListViewDataItem> itemsToRemove = UpdateQuantities();
			if (itemsToRemove.Any())
				ReBind();
		}

		private List<ListViewDataItem> UpdateQuantities()
		{
			List<ListViewDataItem> itemsToRemove = new List<ListViewDataItem>();
			foreach (ListViewDataItem listViewItem in lsvShoppingCartItems.Items)
			{
				int shoppingCartItemID = (int) lsvShoppingCartItems.DataKeys[listViewItem.DataItemIndex].Value;
				Web.Items.ShoppingCartItem shoppingCartItem = (Web.Items.ShoppingCartItem) Zeus.Context.Persister.Get(shoppingCartItemID);

				TextBox txtQuantity = (TextBox) listViewItem.FindControl("txtQuantity");
				if (string.IsNullOrEmpty(txtQuantity.Text) || txtQuantity.Text == "0")
				{
					Zeus.Context.Persister.Delete(shoppingCartItem);
					itemsToRemove.Add(listViewItem);
				}
				else
				{
					CheckBox chkIsGift = (CheckBox) listViewItem.FindControl("chkIsGift");
					DropDownList ddlGiftWrapTypes = (DropDownList) listViewItem.FindControl("ddlGiftWrapTypes");

					shoppingCartItem.Quantity = Convert.ToInt32(txtQuantity.Text);
					shoppingCartItem.IsGift = chkIsGift.Checked;

					if (!string.IsNullOrEmpty(ddlGiftWrapTypes.SelectedValue))
						shoppingCartItem.GiftWrapType = (Web.Items.GiftWrapType) Zeus.Context.Persister.Get(Convert.ToInt32(ddlGiftWrapTypes.SelectedValue));
					else
						shoppingCartItem.GiftWrapType = null;

					Zeus.Context.Persister.Save(shoppingCartItem);
				}
			}
			return itemsToRemove;
		}

		protected void btnContinueShopping_Click(object sender, EventArgs e)
		{
			UpdateQuantities();

			// If user has just added a product, go back to that product's category page
			if (Request.QueryString["add"] != null)
			{
				Web.Items.StandardProduct product = (Web.Items.StandardProduct) Zeus.Context.Persister.Get(Request.GetRequiredInt("add"));
				Response.Redirect(product.Parent.Url);
			}
			else
			{
				// Otherwise go home.
				Response.Redirect("/");
			}
		}
	}
}
