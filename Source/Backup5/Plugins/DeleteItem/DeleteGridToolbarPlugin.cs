using Ext.Net;
using Zeus.Security;

namespace Zeus.Admin.Plugins.DeleteItem
{
	public class DeleteGridToolbarPlugin : GridToolbarPluginBase
	{
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

		public override void ModifyGrid(Button button, GridPanel gridPanel)
		{
			CheckboxSelectionModel selectionModel = (CheckboxSelectionModel) gridPanel.SelectionModel.Primary;
			selectionModel.Listeners.RowSelect.Handler = string.Format("{0}.enable();", button.ClientID);
			selectionModel.Listeners.RowDeselect.Handler = string.Format("if (!{0}.hasSelection()) {{{1}.disable();}}", gridPanel.ClientID, button.ClientID);
		}

		public override Button GetToolbarButton(ContentItem contentItem, GridPanel gridPanel)
		{
			Button toolbarButton = new Button
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
				top.Ext.net.DirectMethods.Delete.ShowDialog('Delete Items',
					'Are you sure you wish to delete these items?',
					selectedIDs.join(','), {{ url : '{1}' }});",
				gridPanel.ClientID, Context.AdminManager.GetAdminDefaultUrl());

			return toolbarButton;
		}
	}
}