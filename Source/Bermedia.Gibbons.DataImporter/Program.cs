using System;
using System.Linq;
using Bermedia.Gibbons.Web.Items;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bermedia.Gibbons.DataImporter
{
	public static class Program
	{
		private static Dictionary<Entities.SubCategory, FragranceBeautyCategory> _subCategoriesDictionary;
		private static Dictionary<Entities.Brand, Brand> _brandsDictionary;
		private static Dictionary<Entities.Collection, FragranceBeautyCollection> _collectionsDictionary;
		private static Dictionary<Entities.Product, FragranceBeautyProduct> _productsDictionary;

		private static List<FragranceBeautyBrandCategory> _brandCategories;
		private static List<ProductSize> _productSizes;
		private static List<ProductScent> _productColours;

		public static void Main(string[] args)
		{
			using (Entities.OldGibbonsDataClassesDataContext oldDb = new Entities.OldGibbonsDataClassesDataContext())
			{
				FragranceBeautyDepartment department = Zeus.Context.Persister.Get<FragranceBeautyDepartment>(12);
				ProductColourContainer colourContainer = Zeus.Context.Persister.Get<ProductColourContainer>(49);
				ProductSizeContainer sizeContainer = Zeus.Context.Persister.Get<ProductSizeContainer>(52);
				BrandContainer brandContainer = Zeus.Context.Persister.Get<BrandContainer>(9);

				// Copy SubCategories.
				_subCategoriesDictionary = new Dictionary<Entities.SubCategory, FragranceBeautyCategory>();
				foreach (Entities.SubCategory oldSubCategory in oldDb.SubCategories.Where(sc => sc.Collections.Any(c => c.Products.Any(p => p.Active))))
				{
					FragranceBeautyCategory newCategory = new FragranceBeautyCategory { Parent = department, Name = GetName(oldSubCategory.Name), Title = oldSubCategory.Name };
					_subCategoriesDictionary.Add(oldSubCategory, newCategory);
					Zeus.Context.Persister.Save(newCategory);
				}

				// Copy Brands.
				_brandsDictionary = new Dictionary<Entities.Brand, Brand>();
				foreach (Entities.Brand oldBrand in oldDb.Brands.Where(b => b.Collections.Any(c => c.Products.Any(p => p.Active))))
				{
					Brand newBrand = new Brand { Title = oldBrand.Name, Parent = brandContainer };
					_brandsDictionary.Add(oldBrand, newBrand);
					Zeus.Context.Persister.Save(newBrand);
				}

				// Copy Collections.
				_collectionsDictionary = new Dictionary<Entities.Collection, FragranceBeautyCollection>();
				_brandCategories = new List<FragranceBeautyBrandCategory>();
				foreach (Entities.Collection oldCollection in oldDb.Collections.Where(c => c.Products.Any(p => p.Active)))
				{
					// Create a "brand" category.
					FragranceBeautyBrandCategory newBrandCategory = _brandCategories.SingleOrDefault(c => c.Brand == _brandsDictionary[oldCollection.Brand]);
					if (newBrandCategory == null)
					{
						newBrandCategory = new FragranceBeautyBrandCategory { Name = GetName(oldCollection.Brand.Name), Title = oldCollection.Brand.Name };
						newBrandCategory.Parent = _subCategoriesDictionary[oldCollection.SubCategory];
						newBrandCategory.Brand = _brandsDictionary[oldCollection.Brand];
						Zeus.Context.Persister.Save(newBrandCategory);
						_brandCategories.Add(newBrandCategory);
					}

					FragranceBeautyCollection newCategory = new FragranceBeautyCollection { Name = GetName(oldCollection.Name), Title = oldCollection.Name };
					_collectionsDictionary.Add(oldCollection, newCategory);
					newCategory.AddTo(newBrandCategory);
					Zeus.Context.Persister.Save(newCategory);
				}

				// Copy products.
				_productsDictionary = new Dictionary<Entities.Product, FragranceBeautyProduct>();
				_productSizes = Zeus.Context.Current.Finder.Elements<ProductSize>().ToList();
				_productColours = Zeus.Context.Current.Finder.Elements<ProductScent>().ToList();
				foreach (Entities.Product oldProduct in oldDb.Products.Where(p => p.Active))
				{
					FragranceBeautyProduct newProduct = new FragranceBeautyProduct();
					newProduct.VendorStyleNumber = oldProduct.SKUCode;
					newProduct.Name = GetName(oldProduct.Name);
					newProduct.Title = oldProduct.Name;
					newProduct.RegularPrice = Convert.ToDecimal(oldProduct.Price);
					newProduct.Description = oldProduct.Description;
					//newProduct.Weight = oldProduct.Weight;
					// TODO: Image
					newProduct.Active = oldProduct.Active;

					// Size
					if (!string.IsNullOrEmpty(oldProduct.Size))
					{
						ProductSize newProductSize = _productSizes.SingleOrDefault(ps => ps.Title == oldProduct.Size);
						if (newProductSize == null)
						{
							newProductSize = new ProductSize { Name = GetName(oldProduct.Size), Title = oldProduct.Size, Parent = sizeContainer };
							Zeus.Context.Persister.Save(newProductSize);
							_productSizes.Add(newProductSize);
						}
						newProduct.Children.Add(new ProductSizeLink { ProductSize = newProductSize, Parent = newProduct });
					}

					// Colour
					if (!string.IsNullOrEmpty(oldProduct.Color1))
					{
						ProductScent newProductColour = _productColours.SingleOrDefault(ps => ps.Title == oldProduct.Color1);
						if (newProductColour == null)
						{
							newProductColour = new ProductScent { Title = oldProduct.Color1, HexRef = string.Empty, Parent = colourContainer };
							Zeus.Context.Persister.Save(newProductColour);
							_productColours.Add(newProductColour);
						}
						newProduct.AssociatedColours.Add(newProductColour);
					}

					// TODO: Recommendations

					_productsDictionary.Add(oldProduct, newProduct);
					newProduct.AddTo(_collectionsDictionary[oldProduct.Collection]);
					Zeus.Context.Persister.Save(newProduct);
				}

				// TODO: Newsletter subscriptions
			}
		}

		private static string GetName(string title)
		{
			title = title.ToLower().Replace(' ', '-');
			return Regex.Replace(title, @"[^a-zA-Z0-9\-]", string.Empty);
		}
	}
}
