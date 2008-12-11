using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.FileSystem;
using System.Web.UI.WebControls;
using Zeus.Web.UI;
using Bermedia.Gibbons.Items.Details;

namespace Bermedia.Gibbons.Items
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
		public IFileIdentifier Image
		{
			get { return GetDetail<IFileIdentifier>("Image", null); }
			set { SetDetail<IFileIdentifier>("Image", value); }
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
