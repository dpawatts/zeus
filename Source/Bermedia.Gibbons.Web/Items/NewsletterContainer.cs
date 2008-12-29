using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Newsletter Container", Description = "Container for newsletters")]
	[RestrictParents(typeof(RootItem))]
	public class NewsletterContainer : BaseContentItem
	{
		public NewsletterContainer()
		{
			this.Name = this.Title = "Newsletters";
		}

		protected override string IconName
		{
			get { return "newspaper"; }
		}

		public override string TemplateUrl
		{
			get { return "~/Admin/View.aspx?selected=" + this.Path; }
		}
	}
}
