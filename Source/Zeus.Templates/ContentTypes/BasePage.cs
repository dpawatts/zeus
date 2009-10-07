using Zeus.Design.Editors;
using Zeus.Templates.Design.Editors;

namespace Zeus.Templates.ContentTypes
{
	[DefaultTemplate]
	public abstract class BasePage : BaseContentItem
	{
		[ContentProperty("Page Title", 11, Description = "Used in the &lt;h1&gt; element on the page")]
		[PageTitleEditor]
		public virtual string PageTitle
		{
			get { return GetDetail("PageTitle", Title); }
			set { SetDetail("PageTitle", value); }
		}

		[NameEditor("URL", 20, Required = true, Shared = false)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		[ContentProperty("Visible in Menu", 25)]
		public override bool Visible
		{
			get { return base.Visible; }
			set { base.Visible = value; }
		}

		protected override string IconName
		{
			get { return "page"; }
		}

		public override bool IsPage
		{
			get { return true; }
		}
	}
}