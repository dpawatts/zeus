using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Gift Card Theme Container", Description = "Container for gift card themes")]
	[RestrictParents(typeof(RootItem))]
	public class GiftCardThemeContainer : StructuralPage
	{
		public GiftCardThemeContainer()
		{
			this.Name = "GiftCardThemes";
			this.Title = "Gift Card Themes";
		}

		protected override string IconName
		{
			get { return "ipod_cast"; }
		}

		protected override string TemplateName
		{
			get { return "GiftCardThemes"; }
		}
	}
}
