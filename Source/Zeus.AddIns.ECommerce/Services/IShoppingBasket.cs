using System.Collections.Generic;
using Zeus.AddIns.ECommerce.ContentTypes.Data;

namespace Zeus.AddIns.ECommerce.Services
{
	public interface IShoppingBasket
	{
		IEnumerable<IShoppingBasketItem> Items { get; }
		DeliveryMethod DeliveryMethod { get; set; }
		int TotalItemCount { get; }
		decimal SubTotalPrice { get; }
		decimal TotalPrice { get; }

		Address BillingAddress { get; set; }
		Address ShippingAddress { get; set; }

		PaymentCard PaymentCard { get; set; }

		string EmailAddress { get; set; }
		string TelephoneNumber { get; set; }
		string MobileTelephoneNumber { get; set; }
	}
}