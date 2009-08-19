using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType]
	[RestrictParents(typeof(Category))]
	public class Product : BasePage
	{
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
	}
}