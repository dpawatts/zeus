using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "Newsletter Subscription")]
	[RestrictParents(typeof(NewsletterSubscriptionContainer))]
	public class NewsletterSubscription : BaseContentItem
	{
		[LiteralDisplayer(Title = "Email Address")]
		[TextBoxEditor("Email Address", 210)]
		public string EmailAddress
		{
			get { return GetDetail<string>("EmailAddress", string.Empty); }
			set { SetDetail<string>("EmailAddress", value); }
		}

		[LiteralDisplayer(Title = "First Name")]
		[TextBoxEditor("First Name", 220)]
		public string FirstName
		{
			get { return GetDetail<string>("FirstName", string.Empty); }
			set { SetDetail<string>("FirstName", value); }
		}

		[LiteralDisplayer(Title = "Last Name")]
		[TextBoxEditor("Last Name", 230)]
		public string LastName
		{
			get { return GetDetail<string>("LastName", string.Empty); }
			set { SetDetail<string>("LastName", value); }
		}

		[TextBoxEditor("Contact No", 240)]
		public string ContactNo
		{
			get { return GetDetail<string>("ContactNo", string.Empty); }
			set { SetDetail<string>("ContactNo", value); }
		}

		protected override string IconName
		{
			get { return "email"; }
		}
	}
}
