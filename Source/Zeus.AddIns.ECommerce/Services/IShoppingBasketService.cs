using System.Collections.Generic;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.PaymentGateways;

namespace Zeus.AddIns.ECommerce.Services
{
	public interface IShoppingBasketService
	{
		bool IsValidVariationPermutation(Product product, IEnumerable<Variation> variations);
		IEnumerable<PaymentCardType> GetSupportedCardTypes();

		void AddItem(Shop shop, Product product, IEnumerable<Variation> variations);
		void ClearBasket(Shop shop);
		IShoppingBasket GetBasket(Shop shop);
		void RemoveItem(Shop shop, Product product, VariationPermutation variationPermutation);
		void UpdateQuantity(Shop shop, Product product, VariationPermutation variationPermutation, int newQuantity);
		string GetMaskedCardNumber(string cardNumber);
		void SaveBasket(Shop shop);

		Order PlaceOrder(Shop shop, string cardNumber, string cardVerificationCode);
	}
}