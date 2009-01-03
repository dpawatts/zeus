using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(StartPage))]
	public class MyAccount : StructuralPage
	{
		protected override string IconName
		{
			get { return "page"; }
		}

		private string Action
		{
			get;
			set;
		}

		public override ContentItem GetChild(string childName)
		{
			if (childName.Equals("track-orders", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "track-orders";
				return this;
			}
			else if (childName.Equals("personal-details", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "personal-details";
				return this;
			}
			else if (childName.Equals("manage-address-book", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "manage-address-book";
				return this;
			}
			else if (childName.Equals("edit-address", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "edit-address";
				return this;
			}
			else if (childName.Equals("subscribe", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "subscribe";
				return this;
			}
			else if (childName.Equals("unsubscribe", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "unsubscribe";
				return this;
			}
			return base.GetChild(childName);
		}

		public override string TemplateUrl
		{
			get
			{
				switch (Action)
				{
					case "track-orders":
						return "~/UI/Views/TrackOrders.aspx";
					case "personal-details":
						return "~/UI/Views/PersonalDetails.aspx";
					case "manage-address-book":
						return "~/UI/Views/ManageAddressBook.aspx";
					case "edit-address":
						return "~/UI/Views/EditAddress.aspx";
					case "subscribe":
						return "~/UI/Views/CustomerSubscribe.aspx";
					case "unsubscribe":
						return "~/UI/Views/CustomerUnsubscribe.aspx";
					default:
						return base.TemplateUrl;
				}
			}
		}
	}
}
