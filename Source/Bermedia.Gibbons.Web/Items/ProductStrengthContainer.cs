using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Product Strength Container", Description = "Container for product strengths")]
	[RestrictParents(typeof(RootItem))]
	public class ProductStrengthContainer : BaseContentItem
	{
		public ProductStrengthContainer()
		{
			this.Name = "ProductStrengths";
			this.Title = "Product Strengths";
		}

		protected override string IconName
		{
			get { return "color_swatch"; }
		}

		public override string TemplateUrl
		{
			get { return "~/Admin/View.aspx?selected=" + this.Path; }
		}
	}
}
