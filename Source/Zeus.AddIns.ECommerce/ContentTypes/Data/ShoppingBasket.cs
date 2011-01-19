using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ext.Net;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Shopping Basket")]
	[RestrictParents(typeof(ShoppingBasketContainer))]
	public class ShoppingBasket : BaseContentItem, IShoppingBasket
	{
		protected override Icon Icon
		{
			get { return Icon.BasketPut; }
		}

		public virtual Address ShippingAddress
		{
			get { return GetChild("shipping-address") as Address; }
			set
			{
				if (value != null)
				{
					value.Name = "shipping-address";
					value.AddTo(this);
				}
			}
		}

        public virtual Address BillingAddress
		{
			get { return GetChild("billing-address") as Address; }
			set
			{
				if (value != null)
				{
					value.Name = "billing-address";
					value.AddTo(this);
				}
			}
		}

        public virtual PaymentCard PaymentCard
		{
			get { return GetChild("payment-card") as PaymentCard; }
			set
			{
				if (value != null)
				{
					value.Name = "payment-card";
					value.AddTo(this);
				}
			}
		}

        public virtual DeliveryMethod DeliveryMethod
		{
			get { return GetDetail<DeliveryMethod>("DeliveryMethod", null); }
			set { SetDetail("DeliveryMethod", value); }
		}

        public virtual string EmailAddress
		{
			get { return GetDetail("EmailAddress", string.Empty); }
			set { SetDetail("EmailAddress", value); }
		}

        public virtual string TelephoneNumber
		{
			get { return GetDetail("TelephoneNumber", string.Empty); }
			set { SetDetail("TelephoneNumber", value); }
		}

        public virtual string MobileTelephoneNumber
		{
			get { return GetDetail("MobileTelephoneNumber", string.Empty); }
			set { SetDetail("MobileTelephoneNumber", value); }
		}

        public virtual IEnumerable<IShoppingBasketItem> Items
		{
			get { return GetChildren<ShoppingBasketItem>().Cast<IShoppingBasketItem>(); }
		}

        public virtual int TotalItemCount
		{
			get { return Items.Where(p => !p.Product.OutOfStock).Sum(i => i.Quantity); }
		}

        public virtual decimal SubTotalPrice
		{
            get { return Items.Where(p => !p.Product.OutOfStock).Sum(i => i.Product.CurrentPrice * i.Quantity); }
		}

        public virtual decimal TotalVat
        {
            get 
            {
                decimal result = 0;

                // VAT settings are taken from the shop node
                var shop = GetParent().GetParent() as Shop;
                if (shop.VAT > 0)
                {
                    double vat = (double)shop.VAT / 100;
                    double subresult = (double)SubTotalPrice * vat;
                    result = Math.Round(Convert.ToDecimal(subresult), 2);
                }

                return result;
            }
        }

        public decimal SubTotalPricePlusVAT
        {
            get
            {
                // sub total
                decimal result = SubTotalPrice;

                // add VAT
                result += TotalVat;

                return result;
            }
        }

		public decimal TotalPrice
		{
			get
			{
				// sub total
                decimal result = SubTotalPrice;

                // add delivery
				if (DeliveryMethod != null)
                    result += DeliveryMethod.GetPriceForShoppingBasket(this);

                // add VAT
                result += TotalVat;

				return result;
			}
		}
	}
}