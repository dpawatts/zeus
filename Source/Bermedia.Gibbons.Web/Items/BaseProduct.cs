using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.FileSystem;
using System.Web.UI.WebControls;
using Zeus.Web.UI;
using Bermedia.Gibbons.Web.Items.Details;
using Zeus.AddIns.Images.Items.Details;
using SoundInTheory.DynamicImage.Filters;

namespace Bermedia.Gibbons.Web.Items
{
	public abstract class BaseProduct : StructuralPage
	{
		protected override string IconName
		{
			get { return "tag_red"; }
		}

		protected override string TemplateName
		{
			get { return "Product"; }
		}

		#region Public properties

		[TextBoxEditor("Description", 230, TextMode = TextBoxMode.MultiLine, ContainerName = Tabs.General)]
		public virtual string Description
		{
			get { return GetDetail<string>("Description", string.Empty); }
			set { SetDetail<string>("Description", value); }
		}

		[ImageDisplayer(Width = 260, ResizeMode = ResizeMode.UseWidth)]
		[ImageUploadEditor("Image", 240, "~/Upload/Products", ContainerName = Tabs.General)]
		public Zeus.AddIns.Images.Items.Image Image
		{
			get { return GetDetail<Zeus.AddIns.Images.Items.Image>("Image", null); }
			set { SetDetail<Zeus.AddIns.Images.Items.Image>("Image", value); }
		}

		[CheckBoxEditor("Display On Website", "", 250, ContainerName = Tabs.General)]
		public bool Active
		{
			get { return GetDetail<bool>("Active", true); }
			set { SetDetail<bool>("Active", value); }
		}

		#endregion

		public BaseDepartment Department
		{
			get { return this.FindFirstAncestor<BaseDepartment>(); }
		}
	}
}
