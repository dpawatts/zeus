using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis;
using Bermedia.Gibbons.Web.Items;
using SoundInTheory.DynamicImage;
using SoundInTheory.PaymentProcessing;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class CheckoutChangeQuantities : SecurePage<Web.Items.CheckoutChangeQuantities>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
				ReBind();
		}

		protected void btnUpdateQuantities_Click(object sender, EventArgs e)
		{
			List<ListViewDataItem> itemsToRemove = UpdateQuantities();
			if (itemsToRemove.Any())
				ReBind();

			Response.Redirect("~/checkout-summary.aspx");
		}

		private void ReBind()
		{
			// Get ShoppingCart for current user.
			lsvShoppingCartItems.DataSource = this.ShoppingCart.GetChildren();
			lsvShoppingCartItems.DataBind();
		}

		private List<ListViewDataItem> UpdateQuantities()
		{
			List<ListViewDataItem> itemsToRemove = new List<ListViewDataItem>();
			foreach (ListViewDataItem listViewItem in lsvShoppingCartItems.Items)
			{
				int shoppingCartItemID = (int)lsvShoppingCartItems.DataKeys[listViewItem.DataItemIndex].Value;
				Web.Items.ShoppingCartItem shoppingCartItem = (Web.Items.ShoppingCartItem)Zeus.Context.Persister.Get(shoppingCartItemID);

				TextBox txtQuantity = (TextBox)listViewItem.FindControl("txtQuantity");
				if (string.IsNullOrEmpty(txtQuantity.Text) || txtQuantity.Text == "0")
				{
					Zeus.Context.Persister.Delete(shoppingCartItem);
					itemsToRemove.Add(listViewItem);
				}
				else
				{
					shoppingCartItem.Quantity = Convert.ToInt32(txtQuantity.Text);
					Zeus.Context.Persister.Save(shoppingCartItem);
				}
			}
			return itemsToRemove;
		}
	}
}
