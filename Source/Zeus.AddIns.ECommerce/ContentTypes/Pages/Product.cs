using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.Design.Editors;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web.UI;
using Image = Zeus.FileSystem.Images.Image;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType(Name = "BaseProduct")]
	[RestrictParents(typeof(Category))]
	[TabPanel("Images", "Images", 200)]
    [FieldSet("fsMainImage", "Main Image", 100, ContainerName = "Images")]
	public class Product : BasePage
	{
		protected override Icon Icon
		{
			get { return Icon.Package; }
		}

		[ContentProperty("Product Code", 200)]
		public virtual string ProductCode
		{
			get { return GetDetail("ProductCode", string.Empty); }
			set { SetDetail("ProductCode", value); }
		}

		[ContentProperty("Regular Price", 210)]
		[TextBoxEditor(Required = true, EditorPrefixText = "�&nbsp;", TextBoxCssClass = "price")]
		public decimal RegularPrice
		{
			get { return GetDetail("RegularPrice", 0m); }
			set { SetDetail("RegularPrice", value); }
		}

		[ContentProperty("Sale Price", 220)]
		[TextBoxEditor(EditorPrefixText = "�&nbsp;", TextBoxCssClass = "price")]
		public virtual decimal? SalePrice
		{
			get { return GetDetail<decimal?>("SalePrice", null); }
			set { SetDetail("SalePrice", value); }
		}

		public decimal CurrentPrice
		{
			get { return SalePrice ?? RegularPrice; }
		}

        [ContentProperty("Main Image", 230, EditorContainerName = "fsMainImage")]
        [ChildEditor("Main Image", 230)]
        public Image MainImage
		{
			get { return GetChild("MainImage") as Image; }
			set
			{
				if (value != null)
				{
					value.Name = "MainImage";
					value.AddTo(this);
				}
			}
		}

		[MultiImageUploadEditor("Extra Images", 250, ContainerName = "Images")]
		public virtual PropertyCollection ExtraImages
		{
			get { return GetDetailCollection("ExtraImages", true); }
		}

		[VariationConfigurationEditor("Variations", 260)]
		public IEnumerable<VariationConfiguration> VariationConfigurations
		{
			get { return GetChildren<VariationConfiguration>(); }
		}

		public IEnumerable<VariationSet> AvailableVariationSets
		{
			get
			{
				return VariationConfigurations
					.SelectMany(vc => vc.Permutation.Variations.Cast<Variation>().Select(v => v.VariationSet))
					.Distinct();
			}
		}

		public Category CurrentCategory
		{
			get { return (Category) ((Parent is Category) ? Parent : Parent.Parent); }
		}

        [ContentProperty("Item is Out of Stock", 300)]
        public virtual bool OutOfStock
        {
            get { return GetDetail("OutOfStock", false); }
            set { SetDetail("OutOfStock", value); }
        }

        [ContentProperty("VAT Zero Rated", 300, Description="Please check if VAT is not added to this product")]
        public virtual bool VatZeroRated
        {
            get { return GetDetail("VatZeroRated", false); }
            set { SetDetail("VatZeroRated", value); }
        }
	}
}