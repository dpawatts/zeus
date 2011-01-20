using Ext.Net;
using Zeus.AddIns.ECommerce.Services;
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
		public virtual decimal Price
		{
			get { return GetDetail("Price", 0m); }
			set { SetDetail("Price", value); }
		}

        /// <summary>
        /// Return price by default
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public virtual decimal GetPriceForShoppingBasket(IShoppingBasket basket)
        {
            return Price;
        }
	}
}