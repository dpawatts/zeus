using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Country Container", Description = "Container for countries")]
	[RestrictParents(typeof(RootItem))]
	public class CountryContainer : BaseContentItem
	{
		public CountryContainer()
		{
			this.Name = this.Title = "Countries";
		}

		public override string TemplateUrl
		{
			get { return "~/Admin/View.aspx?selected=" + this.Path; }
		}

		protected override string IconName
		{
			get { return "ipod"; }
		}
	}
}
