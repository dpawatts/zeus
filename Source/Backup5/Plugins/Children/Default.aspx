<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.Children.Default" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<ext:ResourceManager runat="server" ID="scriptManager" Theme="Gray" />
	
	<script type="text/javascript">
		var renderIcon = function(value, p, record) {
			return String.format('<img src="{0}" alt="{1}" />',
				value, record.data.Title);
		}

		function prepareToolbar(grid, toolbar, index, record)
		{
			var button = toolbar.items.items[0];
			button.menu = new Ext.ux.menu.StoreMenu({
				baseParams: { node: record.data.ID },
				url: "<%= GetContextMenuLoaderUrl() %>",
				xtype:"storemenu",
				items: []
			});
			button.el.child(button.menuClassTarget).addClass("x-btn-with-menu");
			button.menu.on("show", button.onMenuShow, button);
			button.menu.on("hide", button.onMenuHide, button);
		}
	</script>
	
	<ext:Store ID="exsDataStore" runat="server" OnRefreshData="exsDataStore_RefreshData">
		<Reader>
			<ext:ArrayReader IDProperty="ID">
				<Fields>
					<ext:RecordField Name="ID" Mapping="ID" Type="Int" />
					<ext:RecordField Name="Title" Mapping="Title" />
					<ext:RecordField Name="Name" Mapping="Name" />
					<ext:RecordField Name="Created" Mapping="Created" Type="Date" />
					<ext:RecordField Name="Updated" Mapping="Updated" Type="Date" />
					<ext:RecordField Name="Visible" Mapping="Visible" />
					<ext:RecordField Name="IconUrl" Mapping="IconUrl" />
				</Fields>
			</ext:ArrayReader>
		</Reader>
	</ext:Store>
	
	<ext:Viewport runat="server">
		<Content>
			<ext:FitLayout runat="server">
				<Items>
					<ext:GridPanel runat="server" ID="gpaChildren" StoreID="exsDataStore" StripeRows="true" Border="false">
						<TopBar>
							<ext:Toolbar runat="server" ID="TopToolbar" />
						</TopBar>
						<ColumnModel>
							<Columns>
								<ext:Column ColumnID="Icon" Header="" Width="30" DataIndex="IconUrl">
									<Renderer Fn="renderIcon" />
								</ext:Column>
								<ext:Column ColumnID="Title" Header="Title" Width="200" Sortable="true" DataIndex="Title" />
								<ext:Column ColumnID="Name" Header="Identifier" Width="200" Sortable="true" DataIndex="Name" />
								<ext:Column ColumnID="Created" Header="Created" Width="100" Sortable="true" DataIndex="Created">
									<Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y H:i')" />
								</ext:Column>
								<ext:Column ColumnID="Updated" Header="Updated" Width="100" Sortable="true" DataIndex="Updated">
									<Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y H:i')" />
								</ext:Column>
								<ext:CheckColumn ColumnID="Visible" Header="Visible" Width="50" Sortable="true" DataIndex="Visible" />
								<ext:CommandColumn>
									<Commands>
										<ext:GridCommand Text="Actions" />
									</Commands>
									<PrepareToolbar Fn="prepareToolbar" />
								</ext:CommandColumn>
							</Columns>
						</ColumnModel>
						<SelectionModel>
							<ext:CheckboxSelectionModel />
						</SelectionModel>
						<LoadMask ShowMask="true" />
						<BottomBar>
							<ext:PagingToolBar runat="server" PageSize="10" StoreID="exsDataStore" />
						</BottomBar>
					</ext:GridPanel>
				</Items>
			</ext:FitLayout>
		</Content>
	</ext:Viewport>
</asp:Content>