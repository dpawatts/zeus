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
					default:
						return base.TemplateUrl;
				}
			}
		}
	}
}
