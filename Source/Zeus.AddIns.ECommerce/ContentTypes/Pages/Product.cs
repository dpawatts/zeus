using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.FileSystem.Images;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType(Name = "BaseProduct")]
	[RestrictParents(typeof(Category))]
	public class Product : BasePage
	{
		[ContentProperty("Product Code", 200)]
		public string ProductCode
		{
			get { return GetDetail("ProductCode", string.Empty); }
			set { SetDetail("ProductCode", value); }
		}

		[ContentProperty("Regular Price", 210)]
		[TextBoxEditor(Required = true, EditorPrefixText = "£&nbsp;", TextBoxCssClass = "price")]
		public decimal RegularPrice
		{
			get { return GetDetail("RegularPrice", 0m); }
			set { SetDetail("RegularPrice", value); }
		}

		[ContentProperty("Sale Price", 220)]
		[TextBoxEditor(EditorPrefixText = "£&nbsp;", TextBoxCssClass = "price")]
		public decimal? SalePrice
		{
			get { return GetDetail<decimal?>("SalePrice", null); }
			set { SetDetail("SalePrice", value); }
		}

		public decimal CurrentPrice
		{
			get { return SalePrice ?? RegularPrice; }
		}

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

		[MultiImageUploadEditor("Extra Images", 250)]
		public PropertyCollection ExtraImages
		{
			get { return GetDetailCollection("ExtraImages", true); }
		}
	}
}