using System;
using Zeus.ContentTypes.Properties;
using Zeus;
using Zeus.Web.UI.WebControls;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Items
{
	public abstract class StructuralPage : BaseContentItem
	{
		[TextBoxEditor("Title", 10, ContainerName = Tabs.General, Required = true)]
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
	}
}
