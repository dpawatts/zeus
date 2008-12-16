using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.AddIns.Images.Items.Details;
using SoundInTheory.DynamicImage.Filters;

namespace Bermedia.Gibbons.Web.Items
{
	public abstract class BaseCategory : StructuralPage
	{
		[ImageDisplayer(Width = 300, ResizeMode = ResizeMode.UseWidth)]
		[ImageUploadEditor("Image", 240, "~/Upload/Categories", ContainerName = Tabs.General)]
		public Zeus.AddIns.Images.Items.Image Image
		{
			get { return GetDetail<Zeus.AddIns.Images.Items.Image>("Image", null); }
			set { SetDetail<Zeus.AddIns.Images.Items.Image>("Image", value); }
		}

		protected override string IconName
		{
			get { return "tag_green"; }
		}

		protected override string TemplateName
		{
			get { return "Category"; }
		}
	}
}
