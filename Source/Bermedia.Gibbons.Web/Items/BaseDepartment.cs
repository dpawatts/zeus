using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	public abstract class BaseDepartment : StructuralPage
	{
		[HtmlTextBoxEditor("Text", 100, ContainerName = Tabs.General)]
		public string Text
		{
			get { return GetDetail<string>("Text", string.Empty); }
			set { SetDetail<string>("Text", value); }
		}

		protected override string IconName
		{
			get { return "tag_purple"; }
		}
	}
}
