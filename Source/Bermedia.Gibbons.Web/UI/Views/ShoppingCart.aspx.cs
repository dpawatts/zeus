using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web;

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
					switch (Request.QueryString["add"])
					{
						case "gc" :
							int imageID = Request.GetRequiredInt("imageid");
							decimal amount = Convert.ToDecimal(Request.GetRequiredString("am"));
							int quantity = Request.GetRequiredInt("qty");

							Web.Items.PersonalisedGiftCardShoppingCartItem shoppingCartItem = new Web.Items.PersonalisedGiftCardShoppingCartItem();
							shoppingCartItem.PricePerUnit = amount;
							shoppingCartItem.Quantity = quantity;
							shoppingCartItem.Image = Zeus.Context.Persister.Get<Zeus.AddIns.Images.Items.Image>(imageID);

							shoppingCartItem.AddTo(this.ShoppingCart);
							Zeus.Context.Persister.Save(shoppingCartItem);

							Response.Redirect("~/shopping-cart.aspx");

							break;
					}
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
				Web.Items.BaseShoppingCartItem shoppingCartItem = (Web.Items.BaseShoppingCartItem) Zeus.Context.Persister.Get(shoppingCartItemID);

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
			if (Request.QueryString["justadded"] != null)
			{
				Web.Items.StandardProduct product = (Web.Items.StandardProduct) Zeus.Context.Persister.Get(Request.GetRequiredInt("justadded"));
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
