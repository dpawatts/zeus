using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Product Strength")]
	[RestrictParents(typeof(ProductStrengthContainer))]
	public class ProductStrength : BaseContentItem
	{
		[LiteralDisplayer(Title = "Name")]
		[TextBoxEditor("Name", 10, Required = true, IsLocallyUnique = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		protected override string IconName
		{
			get { return "color_wheel"; }
		}
	}
}
