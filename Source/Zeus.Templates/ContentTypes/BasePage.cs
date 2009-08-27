using Zeus.Design.Editors;

namespace Zeus.Templates.ContentTypes
{
	[DefaultTemplate]
	public abstract class BasePage : BaseContentItem
	{
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