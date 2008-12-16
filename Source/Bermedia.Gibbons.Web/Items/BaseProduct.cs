using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.FileSystem;
using System.Web.UI.WebControls;
using Zeus.Web.UI;
using Bermedia.Gibbons.Web.Items.Details;

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
		public string Description
		{
			get { return GetDetail<string>("Description", string.Empty); }
			set { SetDetail<string>("Description", value); }
		}

		[ImageUploadEditor("Image", 240, ContainerName = Tabs.General)]
		public string Image
		{
			get { return GetDetail<string>("Image", null); }
			set { SetDetail<string>("Image", value); }
		}

		[CheckBoxEditor("Display On Website", "", 250, ContainerName = Tabs.General)]
		public bool Active
		{
			get { return GetDetail<bool>("Active", true); }
			set { SetDetail<bool>("Active", value); }
		}

		#endregion
	}
}
