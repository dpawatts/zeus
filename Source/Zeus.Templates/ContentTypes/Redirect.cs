using Zeus.Design.Editors;
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
			get { return Isis.Web.Url.ToAbsolute(RedirectItem.Url); }
		}

		public override string HtmlTitle
		{
			get { return base.HtmlTitle; }
			set { base.HtmlTitle = value; }
		}

		public override string PageTitle
		{
			get { return base.PageTitle; }
			set { base.PageTitle = value; }
		}

		public override string MetaKeywords
		{
			get { return base.MetaKeywords; }
			set { base.MetaKeywords = value; }
		}

		public override string MetaDescription
		{
			get { return base.MetaDescription; }
			set { base.MetaDescription = value; }
		}

		[LinkedItemDropDownListEditor("Redirect to", 30, Required = true, TypeFilter = typeof(BasePage), ContainerName = "Content")]
		public virtual ContentItem RedirectItem
		{
			get { return GetDetail<ContentItem>("RedirectItem", null); }
			set { SetDetail("RedirectItem", value); }
		}

		protected override string IconName
		{
			get { return "page_go"; }
		}
	}
}