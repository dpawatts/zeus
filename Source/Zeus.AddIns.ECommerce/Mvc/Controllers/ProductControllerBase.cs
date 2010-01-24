using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Templates.Mvc.Controllers;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	public abstract class ProductControllerBase<TProduct> : ZeusController<TProduct>
		where TProduct : Product
	{
		private readonly IShoppingBasketService _shoppingBasketService;

		protected ProductControllerBase(IShoppingBasketService shoppingBasketService)
		{
			_shoppingBasketService = shoppingBasketService;
		}

		[HttpGet]
		public ActionResult AddToShoppingBasket()
		{
			return AddItem(null);
		}

		[HttpPost]
		public ActionResult AddToShoppingBasket(FormCollection formValues)
		{
			// Get variation configuration.
			List<Variation> variations = new List<Variation>();
			foreach (VariationSet variationSet in CurrentItem.AvailableVariationSets)
			{
				string selectedValue = formValues["variationSet" + variationSet.ID];
				if (!string.IsNullOrEmpty(selectedValue.Trim()))
				{
					int variationID = Convert.ToInt32(selectedValue);
					Variation variation = Engine.Persister.Get<Variation>(variationID);
					variations.Add(variation);
				}
			}

			// Check that this is a valid variation configuration.
			if (!_shoppingBasketService.IsValidVariationPermutation(CurrentItem, variations))
			{
				TempData["ErrorMessage"] = "This product is not available in this configuration.";
				return RedirectToParentPage();
			}

			return AddItem(variations);
		}

		private ActionResult AddItem(List<Variation> variations)
		{
			Shop shop = (Shop) CurrentItem.CurrentCategory.Parent;
			_shoppingBasketService.AddItem(shop, CurrentItem, variations);

			// Redirect to shopping basket page, if one exists.
			ShoppingBasketPage shoppingBasketPage = shop.ShoppingBasketPage;
			if (shoppingBasketPage != null)
				return Redirect(shoppingBasketPage.Url);
			return Redirect(CurrentItem.Url);
		}
	}
}