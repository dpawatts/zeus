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
		[TextBoxEditor("Name", 10, Required = true, IsLocallyUnique = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[ColourDisplayer(Title = "Colour")]
		[ColourEditor("Hex Ref", 20, Required = true, Description = "If you cannot find the hexadecimal code for the colour or are unfamiliar with hexadecimal codes then <a href=\"http://www.colorschemer.com/online.html\" target=\"_blank\">click here</a> to find an alternative converter that allows you to either pick a colour from a palette or convert your RGB references.")]
		public string HexRef
		{
			get { return GetDetail<string>("HexRef", "FFFFFF"); }
			set { SetDetail<string>("HexRef", value); }
		}

		protected override string IconName
		{
			get { return "color_wheel"; }
		}
	}
}
