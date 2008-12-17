using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Product Color")]
	[RestrictParents(typeof(ProductColourContainer))]
	public class ProductColour : BaseContentItem
	{
		[LiteralDisplayer(Title = "Name")]
		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[ColourDisplayer(Title = "Colour")]
		[ColourEditor("Hex Ref", 20, Required = true)]
		public string HexRef
		{
			get { return GetDetail<string>("HexRef", "FFFFFF"); }
			set { SetDetail<string>("HexRef", value); }
		}

		protected override string IconName
		{
			get { return "ipod"; }
		}
	}
}
