using Zeus.Design.Editors;
using Zeus.Globalization;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	/// <summary>
	/// Redirects to somewhere else. Used as a placeholder in the menu.
	/// </summary>
	[ContentType("Redirect", "Redirect", "Redirects to another page or an external address.", "", 40)]
	[RestrictParents(typeof(BasePage))]
	public class Redirect : BasePage
	{
		public override string Url
		{
			get { return BaseLibrary.Web.Url.ToAbsolute(GetRedirectItem().Url); }
		}

		protected virtual ContentItem GetRedirectItem()
		{
			return Context.Current.LanguageManager.GetTranslation(RedirectItem, ContentLanguage.PreferredCulture.Name) ?? RedirectItem;
		}

		[LinkedItemDropDownListEditor("Redirect to", 30, Required = true, TypeFilter = typeof(BasePage), ContainerName = "Content")]
		public virtual ContentItem RedirectItem
		{
			get { return GetDetail<ContentItem>("RedirectItem", null); }
			set { SetDetail("RedirectItem", value); }
		}

		[ContentProperty("Check Children for Navigation State", 40, Description = "For example, uncheck this for a 'Home' redirect item, otherwise you will have two highlighted items in the navigation.")]
		public virtual bool CheckChildrenForNavigationState
		{
			get { return GetDetail("CheckChildrenForNavigationState", true); }
			set { SetDetail("CheckChildrenForNavigationState", value); }
		}

		protected override string IconName
		{
			get { return "page_go"; }
		}
	}
}