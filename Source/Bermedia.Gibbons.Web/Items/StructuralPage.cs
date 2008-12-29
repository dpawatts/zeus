using System;
using Zeus.ContentTypes.Properties;
using Zeus;
using Zeus.Web.UI.WebControls;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.Items
{
	[TabPanel(Tabs.General, "General", 0)]
	public abstract class StructuralPage : BaseContentItem
	{
		[TextBoxEditor("Title", 10, ContainerName = Tabs.General, Required = true)]
		[LiteralDisplayer]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[NameEditor("Name", 20, ContainerName = Tabs.General)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		[CheckBoxEditor("Visible", "", 25, ContainerName = Tabs.General)]
		public override bool Visible
		{
			get { return base.Visible; }
			set { base.Visible = value; }
		}

		public override bool IsPage
		{
			get { return true; }
		}
	}
}
