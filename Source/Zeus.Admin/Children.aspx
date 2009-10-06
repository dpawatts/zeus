<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Children.aspx.cs" Inherits="Zeus.Admin.Children" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<ext:ScriptManager runat="server" ID="scriptManager" Theme="Gray" />
	
	<script type="text/javascript">
		var renderIcon = function(value, p, record) {
			return String.format('<img src="{0}" alt="{1}" />',
				value, record.data.Title);
		}
	</script>
	
	<ext:Store ID="exsDataStore" runat="server" OnRefreshData="exsDataStore_RefreshData">
		<Reader>
			<ext:ArrayReader ReaderID="ID">
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
	
	<ext:ViewPort runat="server">
		<Body>
			<ext:FitLayout runat="server">
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
							<ext:CommandColumn Width="80">
								<Commands>
									<ext:GridCommand Text="Actions">
										<Menu>
											<Items>
												<ext:MenuCommand Text="Item" Icon="ArrowRight" CommandName="Item" />
												<ext:MenuCommand Text="Submenu" Icon="ArrowRight">
													<Menu>
														<Items>
															<ext:MenuCommand Text="Item" Icon="ArrowRight" CommandName="Item" />        
															<ext:MenuCommand Text="Item" Icon="ArrowRight" CommandName="Item" />        
														</Items>
													</Menu>
												</ext:MenuCommand>
											</Items>
										</Menu>
										<ToolTip Text="Menu" />
									</ext:GridCommand>
								</Commands>
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
			</ext:FitLayout>
		</Body>
	</ext:ViewPort>
</asp:Content>