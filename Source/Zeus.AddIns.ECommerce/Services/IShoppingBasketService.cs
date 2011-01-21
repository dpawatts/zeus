using System.Collections.Generic;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;

namespace Zeus.AddIns.ECommerce.Services
{
	public interface IShoppingBasketService
	{
		bool IsValidVariationPermutation(Product product, IEnumerable<Variation> variations);

		void AddItem(Shop shop, Product product, IEnumerable<Variation> variations);
		void ClearBasket(Shop shop);
		ShoppingBasket GetBasket(Shop shop);
		void RemoveItem(Shop shop, Product product, VariationPermutation variationPermutation);
		void UpdateQuantity(Shop shop, Product product, VariationPermutation variationPermutation, int newQuantity);
		void SaveBasket(Shop shop);

        void CalculateBasketTotal(object sender, Zeus.CancelItemEventArgs e);
	}
}