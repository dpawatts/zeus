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
	public partial class CheckoutSummary : SecurePage<Web.Items.CheckoutSummary>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				lsvShoppingCartItems.DataSource = this.ShoppingCart.Children;
				lsvShoppingCartItems.DataBind();
			}
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			// Create order in database.
			Order order = new Order();
			order.AddTo(this.Customer);
			order.DatePlaced = DateTime.Now;
			Zeus.Context.Persister.Save(order);

			order.BillingAddress = (Address) Zeus.Context.Persister.Copy(this.ShoppingCart.BillingAddress, order);
			order.ShippingAddress = (Address) Zeus.Context.Persister.Copy(this.ShoppingCart.ShippingAddress, order);
			order.DeliveryType = this.ShoppingCart.DeliveryType;
			order.DeliveryPrice = this.ShoppingCart.DeliveryType.GetPrice(this.ShoppingCart);
			Zeus.Context.Persister.Save(order);

			foreach (ShoppingCartItem shoppingCartItem in this.ShoppingCart.Children)
			{
				OrderItem orderItem = new OrderItem();
				orderItem.AddTo(order);

				orderItem.Colour = shoppingCartItem.Colour;
				orderItem.GiftWrapType = shoppingCartItem.GiftWrapType;
				orderItem.IsGift = shoppingCartItem.IsGift;
				orderItem.PricePerUnit = shoppingCartItem.PricePerUnit;
				orderItem.Product = shoppingCartItem.Product;
				orderItem.Quantity = shoppingCartItem.Quantity;
				orderItem.Size = shoppingCartItem.Size;

				Zeus.Context.Persister.Save(orderItem);
			}
			
			Payment payment = new Payment(order.ID.ToString(), Convert.ToDecimal(order.TotalPrice), -1);

			string authorisationHtml = PaymentManager.GetAuthorisationHtml(
				payment,
				false,
				Request.Url.GetLeftPart(UriPartial.Authority) + "/checkout-confirmation.aspx",
				CheckoutData.PaymentCardNumber,
				CheckoutData.PaymentCardExpiryDate,
				CheckoutData.PaymentCvv2,
				order.BillingAddress.AddressLine1,
				order.BillingAddress.Zip);

			// Write out authorisation html directly.
			Response.Clear();
			Response.Write(authorisationHtml);
			Response.End();
		}
	}
}
