using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Gift Wrap Type Container", Description = "Container for gift wrap types")]
	[RestrictParents(typeof(RootItem))]
	public class GiftWrapTypeContainer : BaseContentItem
	{
		public GiftWrapTypeContainer()
		{
			this.Name = "GiftWrapTypes";
			this.Title = "Gift Wrap Types";
		}

		protected override string IconName
		{
			get { return "package_green"; }
		}

		public override string TemplateUrl
		{
			get { return "~/Admin/View.aspx?selected=" + this.Path; }
		}
	}
}
