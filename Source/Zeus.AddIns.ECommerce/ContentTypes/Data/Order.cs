using System.Collections.Generic;
using System.Linq;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType]
	[RestrictParents(typeof(OrderContainer))]
	public class Order : BaseContentItem
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(Order), "Zeus.AddIns.ECommerce.Icons.basket_put.png"); }
		}

		public Address ShippingAddress
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

		public Address BillingAddress
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

		public PaymentCard PaymentCard
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

		public OrderStatus Status
		{
			get { return GetDetail<OrderStatus>("Status", OrderStatus.Unpaid); }
			set { SetDetail("Status", value); }
		}

		public DeliveryMethod DeliveryMethod
		{
			get { return GetDetail<DeliveryMethod>("DeliveryMethod", null); }
			set { SetDetail("DeliveryMethod", value); }
		}

		public decimal DeliveryPrice
		{
			get { return GetDetail("DeliveryPrice", 0m); }
			set { SetDetail("DeliveryPrice", value); }
		}

		public string EmailAddress
		{
			get { return GetDetail("EmailAddress", string.Empty); }
			set { SetDetail("EmailAddress", value); }
		}

		public string TelephoneNumber
		{
			get { return GetDetail("TelephoneNumber", string.Empty); }
			set { SetDetail("TelephoneNumber", value); }
		}

		public string MobileTelephoneNumber
		{
			get { return GetDetail("MobileTelephoneNumber", string.Empty); }
			set { SetDetail("MobileTelephoneNumber", value); }
		}

		public IEnumerable<OrderItem> Items
		{
			get { return GetChildren<OrderItem>(); }
		}

		public int TotalItemCount
		{
			get { return Items.Sum(i => i.Quantity); }
		}

		public decimal SubTotalPrice
		{
			get { return Items.Sum(i => i.LineTotal); }
		}

		public decimal TotalPrice
		{
			get { return SubTotalPrice + DeliveryPrice; }
		}
	}
}