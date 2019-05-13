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
            //get { return Items.Where(p => !p.Product.OutOfStock).Sum(i => (i.VariationPermutation != null && i.VariationPermutation.Variations.Any(v => ((Variation)v).PriceOverride != null) ? ((Variation)i.VariationPermutation.Variations.First(v => ((Variation)v).PriceOverride != null)).PriceOverride.Value : i.Product.CurrentPrice) * i.Quantity); }
            get { return Items.Where(p => !p.Product.OutOfStock).Sum(i => ((i.VariationPermutation != null && i.VariationPermutation.PriceOverride != null) ? i.VariationPermutation.PriceOverride.Value : i.Product.CurrentPrice) * i.Quantity); }
        }

        public virtual decimal SubTotalPricePostDiscount
        {
            //get { return Items.Where(p => !p.Product.OutOfStock).Sum(i => (i.VariationPermutation != null && i.VariationPermutation.Variations.Any(v => ((Variation)v).PriceOverride != null) ? ((Variation)i.VariationPermutation.Variations.First(v => ((Variation)v).PriceOverride != null)).PriceOverride.Value : i.Product.CurrentPrice) * i.Quantity); }
            get {
                decimal amountPreDiscount = Items.Where(p => !p.Product.OutOfStock).Sum(i => ((i.VariationPermutation != null && i.VariationPermutation.PriceOverride != null) ? i.VariationPermutation.PriceOverride.Value : i.Product.CurrentPrice) * i.Quantity); 
                if (DiscountApplied)
                    return Discount.ProcessPrice(amountPreDiscount);
                else
                    return amountPreDiscount;
            }
        }

        public virtual decimal SubTotalPriceForVatCalculation
        {
            get { 
                decimal amountPreDiscount = Items.Where(p => !p.Product.OutOfStock && !p.Product.VatZeroRated).Sum(i => ((i.VariationPermutation != null && i.VariationPermutation.PriceOverride != null) ? i.VariationPermutation.PriceOverride.Value : i.Product.CurrentPrice) * i.Quantity);
                if (DiscountApplied)
                    return Discount.ProcessPrice(amountPreDiscount);
                else
                    return amountPreDiscount;
            }
        }

        public virtual decimal TotalDeliveryPrice
        {
            get { return GetDetail<decimal>("TotalDeliveryPrice", default(decimal)); }
            set { SetDetail("TotalDeliveryPrice", value); }
        }

        public virtual decimal TotalVatPrice
        {
            get { return GetDetail<decimal>("TotalVatPrice", default(decimal)); }
            set { SetDetail("TotalVatPrice", value); }
        }

        public virtual decimal TotalPrice
		{
            get { return GetDetail<decimal>("TotalPrice", default(decimal)); }
            set { SetDetail("TotalPrice", value); }
        }

        public virtual Discount Discount
        {
            get { return GetDetail<Discount>("Discount", default(Discount)); }
            set { SetDetail("Discount", value); }
        }

        public bool DiscountApplied { get { return Discount != null; } }

	}
}