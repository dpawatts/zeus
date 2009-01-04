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

			if (this.ShoppingCart.BillingAddress != null)
				order.BillingAddress = (Address) Zeus.Context.Persister.Copy(this.ShoppingCart.BillingAddress, order);
			order.ShippingAddress = (Address) Zeus.Context.Persister.Copy(this.ShoppingCart.ShippingAddress, order);
			order.DeliveryType = this.ShoppingCart.DeliveryType;
			order.DeliveryPrice = this.ShoppingCart.DeliveryType.GetPrice(this.ShoppingCart);
			order.PaymentMethod = this.ShoppingCart.PaymentMethod;
			Zeus.Context.Persister.Save(order);

			foreach (BaseShoppingCartItem shoppingCartItem in this.ShoppingCart.Children.OfType<BaseShoppingCartItem>())
			{
				BaseOrderItem orderItem = null;
				if (shoppingCartItem is ShoppingCartItem)
				{
					ShoppingCartItem specificShoppingCartItem = (ShoppingCartItem) shoppingCartItem;
					OrderItem specificOrderItem = new OrderItem();
					specificOrderItem.Colour = specificShoppingCartItem.Colour;
					specificOrderItem.Product = specificShoppingCartItem.Product;
					specificOrderItem.Size = specificShoppingCartItem.Size;
					orderItem = specificOrderItem;
				}
				else if (shoppingCartItem is GiftCardShoppingCartItem)
				{
					GiftCardShoppingCartItem specificShoppingCartItem = (GiftCardShoppingCartItem) shoppingCartItem;
					GiftCardOrderItem specificOrderItem = new GiftCardOrderItem();
					specificOrderItem.Description = specificShoppingCartItem.ProductTitle;
					specificOrderItem.Image = specificShoppingCartItem.Image;
					orderItem = specificOrderItem;
				}

				orderItem.GiftWrapType = shoppingCartItem.GiftWrapType;
				orderItem.IsGift = shoppingCartItem.IsGift;
				orderItem.PricePerUnit = shoppingCartItem.PricePerUnit;
				orderItem.Quantity = shoppingCartItem.Quantity;

				orderItem.AddTo(order);
				Zeus.Context.Persister.Save(orderItem);
			}

			switch (this.ShoppingCart.PaymentMethod)
			{
				case PaymentMethod.CreditCard:
					Payment payment = new Payment(order.ID.ToString(), Convert.ToDecimal(order.TotalPrice), 060);

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
					break;
				case PaymentMethod.Corporate:
					order.Status = OrderStatus.New;
					Zeus.Context.Persister.Save(order);
					Response.Redirect("~/checkout-confirmation.aspx");
					break;
			}
		}
	}
}
