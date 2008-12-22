using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Newsletter Subscription Container", Description = "Container for newsletter subscriptions")]
	[RestrictParents(typeof(RootItem))]
	public class NewsletterSubscriptionContainer : BaseContentItem
	{
		public NewsletterSubscriptionContainer()
		{
			this.Name = this.Title = "Newsletter Subscriptions";
		}

		protected override string IconName
		{
			get { return "ipod_cast"; }
		}

		public override string TemplateUrl
		{
			get { return "~/Admin/View.aspx?selected=" + this.Path; }
		}
	}
}
