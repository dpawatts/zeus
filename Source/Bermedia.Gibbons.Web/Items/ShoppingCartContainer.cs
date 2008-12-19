using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Shopping Cart Container", Description = "Container for shopping carts")]
	[RestrictParents(typeof(RootItem))]
	public class ShoppingCartContainer : BaseContentItem
	{
		public ShoppingCartContainer()
		{
			this.Name = this.Title = "Shopping Carts";
		}

		protected override string IconName
		{
			get { return "ipod_cast"; }
		}
	}
}
