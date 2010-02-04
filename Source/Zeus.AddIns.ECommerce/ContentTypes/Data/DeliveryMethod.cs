using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Delivery Method")]
	[RestrictParents(typeof(DeliveryMethodContainer))]
	public class DeliveryMethod : BaseContentItem
	{
		protected override Icon Icon
		{
			get { return Icon.Lorry; }
		}

		[ContentProperty("Price", 200)]
		[TextBoxEditor(Required = true, EditorPrefixText = "£&nbsp;", TextBoxCssClass = "price")]
		public decimal Price
		{
			get { return GetDetail("Price", 0m); }
			set { SetDetail("Price", value); }
		}
	}
}