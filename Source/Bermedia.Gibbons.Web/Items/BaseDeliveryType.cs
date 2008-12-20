using System;
using Zeus;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	public abstract class BaseDeliveryType : BaseContentItem
	{
		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[TextBoxEditor("Description", 20, Required = true)]
		public string Description
		{
			get { return GetDetail<string>("Description", string.Empty); }
			set { SetDetail<string>("Description", value); }
		}

		protected override string IconName
		{
			get { return "world_go"; }
		}

		public abstract decimal GetPrice(ShoppingCart shoppingCart);
	}
}
