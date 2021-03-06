using Ext.Net;
using Zeus.ContentProperties;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType]
	[RestrictParents(typeof(Shop))]
	public class Category : BasePage
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.PageGreen); }
		}

		[XhtmlStringContentProperty("Description", 200)]
		public virtual string Description
		{
			get { return GetDetail("Description", string.Empty); }
			set { SetDetail("Description", value); }
		}

		public string PossessiveTitle
		{
			get { return Title + "'s"; }
		}

		public Shop Shop
		{
			get { return (Shop) Parent; }
		}
	}
}