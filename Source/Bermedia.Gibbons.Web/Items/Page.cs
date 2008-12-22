using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "e.g. Women's Trends")]
	[RestrictParents(typeof(Page), typeof(BaseDepartment))]
	public class Page : StructuralPage
	{
		[HtmlTextBoxEditor("Text", 100, ContainerName = Tabs.General)]
		public string Text
		{
			get { return GetDetail<string>("Text", string.Empty); }
			set { SetDetail<string>("Text", value); }
		}

		[HtmlTextBoxEditor("Navigation Text", 110, ContainerName = Tabs.General)]
		public string NavigationText
		{
			get { return GetDetail<string>("NavigationText", string.Empty); }
			set { SetDetail<string>("NavigationText", value); }
		}

		protected override string IconName
		{
			get { return "page"; }
		}
	}
}
