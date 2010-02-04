using Ext.Net;
using Zeus.Integrity;

namespace Zeus.AddIns
{
	[ContentType("AddIn Container")]
	[RestrictParents(typeof(RootItem))]
	public class AddInContainer : DataContentItem
	{
		public AddInContainer()
		{
			Title = "AddIns";
			Name = "addins";
		}

		protected override Icon Icon
		{
			get { return Icon.PluginLink; }
		}
	}
}