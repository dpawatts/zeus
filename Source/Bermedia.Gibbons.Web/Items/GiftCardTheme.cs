using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.AddIns.Images.Items.Details;
using SoundInTheory.DynamicImage.Filters;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Title = "Gift card theme")]
	[RestrictParents(typeof(GiftCardThemeContainer))]
	public class GiftCardTheme : BaseContentItem
	{
		[LiteralDisplayer(Title = "Name")]
		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[ImageDisplayer]
		[ImageUploadEditor("Image", 240, "~/Upload/GiftCards/Themes")]
		public Zeus.AddIns.Images.Items.Image Image
		{
			get { return GetDetail<Zeus.AddIns.Images.Items.Image>("Image", null); }
			set { SetDetail<Zeus.AddIns.Images.Items.Image>("Image", value); }
		}

		protected override string IconName
		{
			get { return "ipod"; }
		}
	}
}
