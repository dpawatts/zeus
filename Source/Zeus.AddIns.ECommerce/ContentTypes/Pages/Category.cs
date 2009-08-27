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
			get { return GetIconUrl(typeof(Category), "Zeus.AddIns.ECommerce.Icons.page_green.png"); }
		}

		public string PossessiveTitle
		{
			get { return Title + "'s"; }
		}
	}
}