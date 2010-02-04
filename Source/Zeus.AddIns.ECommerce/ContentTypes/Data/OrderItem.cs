using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[RestrictParents(typeof(Order))]
	public abstract class OrderItem : BaseContentItem
	{
		public abstract string DisplayTitle { get; }

		public int Quantity
		{
			get { return GetDetail("Quantity", 0); }
			set { SetDetail("Quantity", value); }
		}

		public decimal Price
		{
			get { return GetDetail("Price", 0m); }
			set { SetDetail("Price", value); }
		}

		public decimal LineTotal
		{
			get { return Price * Quantity; }
		}
	}
}