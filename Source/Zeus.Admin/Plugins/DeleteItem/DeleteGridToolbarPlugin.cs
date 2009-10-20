using System.Web.UI;
using Coolite.Ext.Web;
using Isis.Web.UI;
using Zeus.Security;

//[assembly: WebResource("Zeus.Admin.Delete.Resources.Ext.ux.zeus.DeleteConfirmation.js", "text/javascript")]

namespace Zeus.Admin.Plugins.DeleteItem
{
	public class DeleteGridToolbarPlugin : GridToolbarPluginBase
	{
		public override string[] RequiredScripts
		{
			get { return new[] { WebResourceUtility.GetUrl(GetType(), "Zeus.Admin.Plugins.DeleteItem.Resources.Ext.ux.zeus.DeleteConfirmation.js") }; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Delete; }
		}

		public override string[] RequiredUserControls
		{
			get { return new[] { GetPageUrl(GetType(), "Zeus.Admin.Plugins.DeleteItem.DeleteUserControl.ascx") }; }
		}

		public override int SortOrder
		{
			get { return 1; }
		}

		public override void ModifyGrid(ToolbarButton button, GridPanel gridPanel)
		{
			CheckboxSelectionModel selectionModel = (CheckboxSelectionModel) gridPanel.SelectionModel.Primary;
			selectionModel.Listeners.RowSelect.Handler = string.Format("{0}.enable();", button.ClientID);
			selectionModel.Listeners.RowDeselect.Handler = string.Format("if (!{0}.hasSelection()) {{{1}.disable();}}", gridPanel.ClientID, button.ClientID);
		}

		public override ToolbarButton GetToolbarButton(ContentItem contentItem, GridPanel gridPanel)
		{
			ToolbarButton toolbarButton = new ToolbarButton
			{
				Text = "Delete",
				Icon = Icon.Delete,
				Disabled = true
			};

			toolbarButton.Listeners.Click.Handler = string.Format(@"
				var selections = {0}.getSelectionModel().getSelections();
				var selectedIDs = [];
				for (var i = 0; i < selections.length; i++)
				{{
					var record = selections[i];
					selectedIDs.push(record.data.ID);
				}}
				window.top.Ext.ux.zeus.DeleteConfirmation.showMultiple(selectedIDs);",
				gridPanel.ClientID);

			return toolbarButton;
		}
	}
}