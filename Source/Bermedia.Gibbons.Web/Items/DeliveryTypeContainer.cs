using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Delivery Type Container", Description = "Container for delivery types")]
	[RestrictParents(typeof(RootItem))]
	public class DeliveryTypeContainer : BaseContentItem
	{
		public DeliveryTypeContainer()
		{
			this.Name = this.Title = "Delivery Types";
		}

		protected override string IconName
		{
			get { return "ipod_cast"; }
		}

		public override string TemplateUrl
		{
			get { return "~/Admin/View.aspx?selected=" + this.Path; }
		}
	}
}
