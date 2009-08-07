using System.Web.UI.WebControls;
using Zeus.Design.Editors;
using Zeus.Web.UI;

namespace Zeus.Templates.ContentTypes
{
	[DefaultTemplate]
	public abstract class BasePage : BaseContentItem
	{
		[NameEditor("URL", 20, Required = true, ContainerName = "Content", Shared = false)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		[ContentProperty("Visible in Menu", 25)]
		[CheckBoxEditor(ContainerName = "Content")]
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