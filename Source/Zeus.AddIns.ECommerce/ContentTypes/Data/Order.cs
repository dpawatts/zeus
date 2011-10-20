using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using Zeus.Integrity;
using Zeus.Security;
using Zeus.Templates.ContentTypes;
using System;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType]
	[RestrictParents(typeof(OrderContainer))]
	public class Order : BaseContentItem
	{
		protected override Icon Icon
		{
			get { return Icon.BasketPut; }
		}

        public User User
		{
			get { return GetDetail<User>("User", null); }
			set { SetDetail("User", value); }
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
			get { return GetDetail("Status", OrderStatus.Unpaid); }
			set { SetDetail("Status", value); }
        }

        public PaymentMethod PaymentMethod
        {
            get { return GetDetail("PaymentMethod", PaymentMethod.SagePay); }
            set { SetDetail("PaymentMethod", value); }
        }

		public string BookingRef
		{
            get { return GetDetail("BookingRef", string.Empty); }
            set { SetDetail("BookingRef", value); }
		}

		public DeliveryMethod DeliveryMethod
		{
			get { return GetDetail<DeliveryMethod>("DeliveryMethod", null); }
			set { SetDetail("DeliveryMethod", value); }
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

        public decimal TotalDeliveryPrice
        {
            get { return GetDetail("TotalDeliveryPrice", 0m); }
            set { SetDetail("TotalDeliveryPrice", value); }
        }

        public virtual decimal TotalVatPrice
        {
            get { return GetDetail("TotalVatPrice", 0m); }
            set { SetDetail("TotalVatPrice", value); }
        }

		public decimal TotalPrice
		{
            get { return GetDetail("TotalPrice", 0m); }
            set { SetDetail("TotalPrice", value); }
		}
	}
}