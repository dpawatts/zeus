using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Isis;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(Customer))]
	public class Order : ShoppingCart
	{
		public DateTime DatePlaced
		{
			get { return GetDetail<DateTime>("DatePlaced", DateTime.MinValue); }
			set { SetDetail<DateTime>("DatePlaced", value); }
		}

		public decimal DeliveryPrice
		{
			get { return GetDetail<decimal>("DeliveryPrice", 0); }
			set { SetDetail<decimal>("DeliveryPrice", value); }
		}

		public string TrackingNumber
		{
			get { return GetDetail<string>("TrackingNumber", string.Empty); }
			set { SetDetail<string>("TrackingNumber", value); }
		}

		public OrderStatus Status
		{
			get { return GetDetail<OrderStatus>("Status", OrderStatus.Basket); }
			set { SetDetail<OrderStatus>("Status", value); }
		}

		public DateTime ShippedDate
		{
			get { return GetDetail<DateTime>("ShippedDate", DateTime.MinValue); }
			set { SetDetail<DateTime>("ShippedDate", value); }
		}

		public RefundStatus RefundStatus
		{
			get
			{
				if (this.Children.OfType<OrderItem>().Any(oi => oi.Refunded))
					if (this.Children.OfType<OrderItem>().Any(oi => !oi.Refunded))
						return RefundStatus.PartiallyRefunded;
					else
						return RefundStatus.Refunded;
				else
					return RefundStatus.NotRefunded;
			}
		}

		public PaymentStatus PaymentStatus
		{
			get
			{
				switch (this.Status)
				{
					case OrderStatus.New:
						return PaymentStatus.Authorized;
					case OrderStatus.Collected:
					case OrderStatus.Shipped:
						return PaymentStatus.Received;
					case OrderStatus.Deleted:
						return PaymentStatus.Cancelled;
					default:
						return PaymentStatus.None;
				}
			}
		}

		public string StatusDescription
		{
			get
			{
				switch (this.Status)
				{
					case OrderStatus.Shipped :
						return "Shipped on " + this.ShippedDate;
					case OrderStatus.Collected :
						return "Picked up on " + this.ShippedDate;
					default :
						return this.Status.GetDescription();
				}
			}
		}

		protected override string TemplateName
		{
			get { return "ViewOrder"; }
		}
	}
}
