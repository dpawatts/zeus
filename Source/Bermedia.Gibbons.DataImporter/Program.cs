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
		private static Dictionary<Entities.Collection, FragranceBeautyCategory> _collectionsDictionary;
		private static Dictionary<Entities.Product, FragranceBeautyProduct> _productsDictionary;

		private static List<FragranceBeautyCategory> _brandCategories;
		private static List<ProductSize> _productSizes;
		private static List<ProductColour> _productColours;

		public static void Main(string[] args)
		{
			using (Entities.OldGibbonsDataClassesDataContext oldDb = new Entities.OldGibbonsDataClassesDataContext())
			{
				FragranceBeautyDepartment department = Zeus.Context.Persister.Get<FragranceBeautyDepartment>(12);

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
					Brand newBrand = new Brand { Title = oldBrand.Name };
					_brandsDictionary.Add(oldBrand, newBrand);
					Zeus.Context.Persister.Save(newBrand);
				}

				// Copy Collections.
				_collectionsDictionary = new Dictionary<Entities.Collection, FragranceBeautyCategory>();
				_brandCategories = new List<FragranceBeautyCategory>();
				foreach (Entities.Collection oldCollection in oldDb.Collections.Where(c => c.Products.Any(p => p.Active)))
				{
					// Create a "brand" category.
					FragranceBeautyCategory newBrandCategory = _brandCategories.SingleOrDefault(c => c.Brand == _brandsDictionary[oldCollection.Brand]);
					if (newBrandCategory == null)
					{
						newBrandCategory = new FragranceBeautyCategory { Name = GetName(oldCollection.Brand.Name), Title = oldCollection.Brand.Name };
						newBrandCategory.Parent = _subCategoriesDictionary[oldCollection.SubCategory];
						newBrandCategory.Brand = _brandsDictionary[oldCollection.Brand];
						Zeus.Context.Persister.Save(newBrandCategory);
						_brandCategories.Add(newBrandCategory);
					}

					FragranceBeautyCategory newCategory = new FragranceBeautyCategory { Name = GetName(oldCollection.Name), Title = oldCollection.Name };
					_collectionsDictionary.Add(oldCollection, newCategory);
					newCategory.AddTo(newBrandCategory);
					Zeus.Context.Persister.Save(newCategory);
				}

				// Copy products.
				_productsDictionary = new Dictionary<Entities.Product, FragranceBeautyProduct>();
				_productSizes = new List<ProductSize>();
				_productColours = new List<ProductColour>();
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
					ProductSize newProductSize = _productSizes.SingleOrDefault(ps => ps.Name == oldProduct.Size);
					if (newProductSize == null)
					{
						newProductSize = new ProductSize { Name = oldProduct.Size };
						Zeus.Context.Persister.Save(newProductSize);
						_productSizes.Add(newProductSize);
					}
					newProduct.Children.Add(new ProductSizeLink { ProductSize = newProductSize });

					// Colour
					if (!string.IsNullOrEmpty(oldProduct.Color1))
					{
						ProductColour newProductColour = _productColours.SingleOrDefault(ps => ps.Title == oldProduct.Color1);
						if (newProductColour == null)
						{
							newProductColour = new ProductColour { Title = oldProduct.Color1, HexRef = "#" };
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
