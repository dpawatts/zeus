using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Mvc.ViewModels;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	[Controls(typeof(Product), AreaName = ECommerceWebPackage.AREA_NAME)]
	public class ProductController : ZeusController<Product>
	{
		private readonly IShoppingBasketService _shoppingBasketService;

		public ProductController(IShoppingBasketService shoppingBasketService)
		{
			_shoppingBasketService = shoppingBasketService;
		}

		public override ActionResult Index()
		{
			return View(new ProductViewModel(CurrentItem,
				CurrentItem.CurrentCategory,
				CurrentItem.CurrentCategory.GetChildren<Subcategory>()));
		}

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