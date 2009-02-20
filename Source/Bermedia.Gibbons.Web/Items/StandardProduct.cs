using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus.Web.UI;
using Zeus.ContentTypes.Properties;
using Bermedia.Gibbons.Web.Items.Details;

namespace Bermedia.Gibbons.Web.Items
{
	[TabPanel(Tabs.Colours, "Colors", 1)]
	[TabPanel(Tabs.Sizes, "Sizes", 2)]
	[TabPanel(Tabs.Recommendations, "Recommendations", 3)]
	public abstract class StandardProduct : BaseProduct
	{
		[TextBoxEditor("Vendor Style Number", 200, ContainerName = Tabs.General, Required = true)]
		public string VendorStyleNumber
		{
			get { return GetDetail<string>("VendorStyleNumber", string.Empty); }
			set { SetDetail<string>("VendorStyleNumber", value); }
		}

		public abstract Brand Brand
		{
			get;
			set;
		}

		public string DisplayTitle
		{
			get { return ((Brand != null) ? Brand.Title + " " : string.Empty) + Title; }
		}

		public virtual string SubTitle
		{
			get { return string.Empty; }
		}

		[TextBoxEditor("Regular Price", 210, ContainerName = Tabs.General, Required = true)]
		public decimal RegularPrice
		{
			get { return GetDetail<decimal>("RegularPrice", 0); }
			set { SetDetail<decimal>("RegularPrice", value); }
		}

		[TextBoxEditor("Sale Price", 220, ContainerName = Tabs.General)]
		public decimal? SalePrice
		{
			get { return GetDetail<decimal?>("SalePrice", null); }
			set { SetDetail<decimal?>("SalePrice", value); }
		}

		public decimal CurrentPrice
		{
			get { return (decimal) (this.SalePrice ?? this.RegularPrice); }
		}

		[CheckBoxEditor("Exclusive", "", 260, ContainerName = Tabs.General)]
		public bool Exclusive
		{
			get { return GetDetail<bool>("Exclusive", false); }
			set { SetDetail<bool>("Exclusive", value); }
		}

		[CheckBoxEditor("Gift Item?", "", 270, ContainerName = Tabs.General)]
		public bool GiftItem
		{
			get { return GetDetail<bool>("GiftItem", false); }
			set { SetDetail<bool>("GiftItem", value); }
		}

		[LinkedItemDropDownListEditor("Free Gift Product", 290, TypeFilter = typeof(FreeGiftProduct), ContainerName = Tabs.General)]
		//[ComboBoxEditor("Free Gift Product", 290, ContainerName = Tabs.General)]
		public FreeGiftProduct FreeGiftProduct
		{
			get { return GetDetail<FreeGiftProduct>("FreeGiftProduct", null); }
			set { SetDetail<FreeGiftProduct>("FreeGiftProduct", value); }
		}

		[ProductColoursEditor("Associated Colors", 300, ContainerName = Tabs.Colours)]
		public virtual DetailCollection AssociatedColours
		{
			get { return GetDetailCollection("AssociatedColours", true); }
		}

		[ChildrenEditor("Associated Sizes", 400, TypeFilter = typeof(ProductSizeLink), ContainerName = Tabs.Sizes)]
		public IList<ProductSizeLink> AssociatedSizes
		{
			get { return GetChildren<ProductSizeLink>(); }
		}

		[LinkedItemsEditor("Recommendations", 500, typeof(StandardProduct), ContainerName = Tabs.Recommendations)]
		public DetailCollection Recommendations
		{
			get { return GetDetailCollection("Recommendations", true); }
		}
	}
}
