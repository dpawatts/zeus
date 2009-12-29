<%@ Page Title="Moderate Comments" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.AddIns.Blogs.Admin.Plugins.ModerateComments.Default" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<ext:ScriptManager runat="server" ID="scriptManager" Theme="Gray" />
	
	<script type="text/javascript">
		var renderAuthor = function(value, p, record) {
			return String.format('{0}<br /><a href="{1}" target="_blank">{1}</a>',
				value, record.data.AntiSpamAuthorUrl);
		}

		var renderComment = function(value, p, record) {
			return String.format('<span style="color:#555555">Submitted on {0}</span><br />{1}',
				Ext.util.Format.date(record.data.Created, 'd/m/Y H:i'), value);
		}

		var renderInResponseTo = function(value, p, record) {
			return String.format('<a href="{0}">{1}</a>',
				record.data.PostUrl, value);
		}

		var renderStatus = function(value, p, record)
		{
			if (record.data.Spam)
				return "Spam";
			else if (record.data.Approved)
				return "Approved";
			else
				return "Pending";
		}
	</script>
	
	<ext:Store ID="exsDataStore" runat="server" OnRefreshData="exsDataStore_RefreshData">
		<Reader>
			<ext:ArrayReader>
				<Fields>
					<ext:RecordField Name="ID" Mapping="ID" Type="Int" />
					<ext:RecordField Name="AntiSpamAuthorName" Mapping="AntiSpamAuthorName" />
					<ext:RecordField Name="AntiSpamAuthorUrl" Mapping="AntiSpamAuthorUrl" />
					<ext:RecordField Name="AntiSpamContent" Mapping="AntiSpamContent" />
					<ext:RecordField Name="Created" Mapping="Created" Type="Date" />
					<ext:RecordField Name="PostTitle" Mapping="PostTitle" />
					<ext:RecordField Name="PostUrl" Mapping="PostUrl" />
					<ext:RecordField Name="Spam" Mapping="Spam" />
					<ext:RecordField Name="Approved" Mapping="Approved" />
				</Fields>
			</ext:ArrayReader>
		</Reader>
	</ext:Store>
	
	<ext:ViewPort runat="server">
		<Body>
			<ext:FitLayout runat="server">
				<ext:GridPanel runat="server" ID="gpaComments" StoreID="exsDataStore" StripeRows="true" AutoExpandColumn="Comment" Border="false">
					<TopBar>
						<ext:Toolbar>
							<Items>
								<ext:Button runat="server" ID="btnApprove" Text="Approve" Icon="Tick" Disabled="true">
									<AjaxEvents>
										<Click OnEvent="ApproveFeedback">
											<ExtraParams>
												<ext:Parameter Name="Values" Value="Ext.encode(#{gpaComments}.getRowsValues())" Mode="Raw" />
											</ExtraParams>
										</Click>
									</AjaxEvents>
								</ext:Button>
								<ext:Button runat="server" ID="btnSpam" Text="Spam" Icon="Decline" Disabled="true">
									<AjaxEvents>
										<Click OnEvent="SubmitSpam">
											<ExtraParams>
												<ext:Parameter Name="Values" Value="Ext.encode(#{gpaComments}.getRowsValues())" Mode="Raw" />
											</ExtraParams>
										</Click>
									</AjaxEvents>
								</ext:Button>
								<ext:ToolbarFill />
								<ext:ComboBox runat="server" ID="cboFilterType">
									<Items>
										<ext:ListItem Text="Show all comment types" Value="All" />
										<ext:ListItem Text="Comments" Value="Comments" />
										<ext:ListItem Text="Pingbacks" Value="Pingbacks" />
									</Items>
									<AjaxEvents>
										<Select OnEvent="cboFilterType_Select" />
									</AjaxEvents>
								</ext:ComboBox>
								<ext:ToolbarSpacer />
								<ext:ComboBox runat="server" ID="cboFilterStatus" Width="100">
									<Items>
										<ext:ListItem Text="Pending" Value="Pending" />
										<ext:ListItem Text="Approved" Value="Approved" />
										<ext:ListItem Text="Spam" Value="Spam" />
									</Items>
									<AjaxEvents>
										<Select OnEvent="cboFilterStatus_Select" />
									</AjaxEvents>
								</ext:ComboBox>
							</Items>
						</ext:Toolbar>
					</TopBar>
					<ColumnModel>
						<Columns>
							<ext:Column ColumnID="Status" Header="Status" Width="100" Sortable="true" DataIndex="Status">
								<Renderer Fn="renderStatus" />
							</ext:Column>
							<ext:Column ColumnID="Author" Header="Author" Width="200" Sortable="true" DataIndex="AntiSpamAuthorName">
								<Renderer Fn="renderAuthor" />
							</ext:Column>
							<ext:Column ColumnID="Comment" Header="Comment" Width="300" Sortable="true" DataIndex="AntiSpamContent">
								<Renderer Fn="renderComment" />
							</ext:Column>
							<ext:Column ColumnID="InResponseTo" Header="In Response To" Width="200" Sortable="true" DataIndex="PostTitle">
								<Renderer Fn="renderInResponseTo" />
							</ext:Column>
						</Columns>
					</ColumnModel>
					<SelectionModel>
						<ext:CheckboxSelectionModel>
							<Listeners>
								<RowSelect Handler="#{btnSpam}.enable();#{btnApprove}.enable();" />
								<RowDeselect Handler="if (!#{gpaComments}.hasSelection()) {#{btnSpam}.disable();#{btnApprove}.disable();}" />
							</Listeners>
						</ext:CheckboxSelectionModel>
					</SelectionModel>
					<LoadMask ShowMask="true" />
					<BottomBar>
						<ext:PagingToolBar runat="server" PageSize="20" StoreID="exsDataStore" />
					</BottomBar>
				</ext:GridPanel>
			</ext:FitLayout>
		</Body>
	</ext:ViewPort>
</asp:Content>