using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.FileSystem;
using System.Web.UI.WebControls;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Items
{
	[ContentType(Description = "e.g. Calvin Klein Striped Socks, Must de Cartier Eau de Toilette")]
	[RestrictParents(typeof(Category))]
	[TabPanel(Tabs.ColoursSizes, "Colors & Sizes", 1)]
	[TabPanel(Tabs.Recommendations, "Recommendations", 2)]
	public class Product : StructuralPage
	{
		protected override string IconName
		{
			get { return "tag_green"; }
		}

		#region Public properties

		[TextBoxEditor("Vendor Style Number", 200, ContainerName = Tabs.General, Required = true)]
		public string VendorStyleNumber
		{
			get { return GetDetail<string>("VendorStyleNumber", string.Empty); }
			set { SetDetail<string>("VendorStyleNumber", value); }
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

		[TextBoxEditor("Description", 230, TextMode = TextBoxMode.MultiLine, ContainerName = Tabs.General)]
		public string Description
		{
			get { return GetDetail<string>("Description", string.Empty); }
			set { SetDetail<string>("Description", value); }
		}

		[ImageUploadEditor("Image", 240, ContainerName = Tabs.General)]
		public IFileIdentifier Image
		{
			get { return GetDetail<IFileIdentifier>("Image", null); }
			set { SetDetail<IFileIdentifier>("Image", value); }
		}

		[CheckBoxEditor("Display On Website", "", 250, ContainerName = Tabs.General)]
		public bool Active
		{
			get { return GetDetail<bool>("Active", true); }
			set { SetDetail<bool>("Active", value); }
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

		// Free Gift?

		[LinkedItemDropDownListEditor("Free Gift Product", 290, TypeFilter = typeof(Product), ExcludeSelf = true, ContainerName = Tabs.General)]
		public Product FreeGiftProduct
		{
			get { return GetDetail<Product>("FreeGiftProduct", null); }
			set { SetDetail<Product>("FreeGiftProduct", value); }
		}

		#endregion
	}
}
