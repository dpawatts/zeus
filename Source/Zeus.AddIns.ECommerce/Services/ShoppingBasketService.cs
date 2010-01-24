using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Persistence;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Services
{
	public class ShoppingBasketService : IShoppingBasketService
	{
		private readonly IPersister _persister;
		private readonly IWebContext _webContext;
		private readonly IFinder _finder;

		public ShoppingBasketService(IPersister persister, IWebContext webContext, IFinder finder)
		{
			_persister = persister;
			_webContext = webContext;
			_finder = finder;
		}

		public bool IsValidVariationPermutation(Product product, IEnumerable<Variation> variations)
		{
			if ((variations == null || !variations.Any()) && (product.VariationConfigurations == null || !product.VariationConfigurations.Any()))
				return true;

			return product.VariationConfigurations.Any(vc => vc.Available
				&& EnumerableUtility.EqualsIgnoringOrder(vc.Permutation.Variations.Cast<Variation>(), variations));
		}

		public void AddItem(Shop shop, Product product, IEnumerable<Variation> variations)
		{
			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop);

			// If card is already in basket, just increment quantity, otherwise create a new item.
			ShoppingBasketItem item = shoppingBasket.GetChildren<ShoppingBasketItem>().SingleOrDefault(i => i.Product == product && ((variations == null && i.Variations == null) || EnumerableUtility.Equals(i.Variations, variations)));
			if (item == null)
			{
				VariationPermutation variationPermutation = null;
				if (variations != null && variations.Any())
				{
					variationPermutation = new VariationPermutation();
					foreach (Variation variation in variations)
						variationPermutation.Variations.Add(variation);
				}
				item = new ShoppingBasketItem { Product = product, VariationPermutation = variationPermutation, Quantity = 1 };
				item.AddTo(shoppingBasket);
			}
			else
			{
				item.Quantity += 1;
			}

			_persister.Save(shoppingBasket);
		}

		public void RemoveItem(Shop shop, Product product, VariationPermutation variationPermutation)
		{
			UpdateQuantity(shop, product, variationPermutation, 0);
		}

		public void UpdateQuantity(Shop shop, Product product, VariationPermutation variationPermutation, int newQuantity)
		{
			if (newQuantity < 0)
				throw new ArgumentOutOfRangeException("newQuantity", "Quantity must be greater than or equal to 0.");

			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop);
			ShoppingBasketItem item = shoppingBasket.GetChildren<ShoppingBasketItem>().SingleOrDefault(i => i.Product == product && i.VariationPermutation == variationPermutation);

			if (item == null)
				return;

			if (newQuantity == 0)
				shoppingBasket.Children.Remove(item);
			else
				item.Quantity = newQuantity;

			_persister.Save(shoppingBasket);
		}

		public IShoppingBasket GetBasket(Shop shop)
		{
			return GetCurrentShoppingBasketInternal(shop);
		}

		public void ClearBasket(Shop shop)
		{
			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop);
			if (shoppingBasket != null)
			{
				_persister.Delete(shoppingBasket);
				_webContext.Response.Cookies.Remove(GetCookieKey(shop));
			}
		}

		private static string GetCookieKey(Shop shop)
		{
			return "ZeusECommerce" + shop.ID;
		}

		private ShoppingBasket GetShoppingBasketFromCookie(Shop shop)
		{
			HttpCookie cookie = _webContext.Request.Cookies[GetCookieKey(shop)];
			if (cookie == null)
				return null;

			string shopperID = cookie.Value;
			return _finder.QueryItems<ShoppingBasket>().SingleOrDefault(sb => sb.Name == shopperID);
		}

		private ShoppingBasket GetCurrentShoppingBasketInternal(Shop shop)
		{
			ShoppingBasket shoppingBasket = GetShoppingBasketFromCookie(shop);

			if (shoppingBasket != null)
			{
				// Check that products in shopping cart still exist, and if not remove those shopping cart items.
				List<ShoppingBasketItem> itemsToRemove = new List<ShoppingBasketItem>();
				foreach (ShoppingBasketItem item in shoppingBasket.GetChildren<ShoppingBasketItem>())
					if (item.Product == null)
						itemsToRemove.Add(item);
				foreach (ShoppingBasketItem item in itemsToRemove)
					shoppingBasket.Children.Remove(item);
				_persister.Save(shoppingBasket);
			}
			else
			{
				shoppingBasket = new ShoppingBasket { Name = Guid.NewGuid().ToString() };
				if (shop.DeliveryMethods != null)
					shoppingBasket.DeliveryMethod = shop.DeliveryMethods.GetChildren<DeliveryMethod>().FirstOrDefault();
				shoppingBasket.AddTo(shop.ShoppingBaskets);
				_persister.Save(shoppingBasket);

				HttpCookie cookie = new HttpCookie(GetCookieKey(shop), shoppingBasket.Name);
				if (shop.PersistentShoppingBaskets)
					cookie.Expires = DateTime.Now.AddYears(1);
				_webContext.Response.Cookies.Add(cookie);
			}

			return shoppingBasket;
		}

		/// <summary>
		/// Masks the credit card using XXXXXX and appends the last 4 digits
		/// </summary>
		public string GetMaskedCardNumber(string cardNumber)
		{
			string result = "****";
			if (cardNumber.Length > 8)
			{
				string lastFour = cardNumber.Substring(cardNumber.Length - 4, 4);
				result = "**** **** **** " + lastFour;
			}
			return result;
		}

		public void SaveBasket(Shop shop)
		{
			_persister.Save(GetCurrentShoppingBasketInternal(shop));
		}
	}
}