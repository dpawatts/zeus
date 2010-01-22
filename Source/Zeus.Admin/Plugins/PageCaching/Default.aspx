<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.PageCaching.Default" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<ext:ScriptManager runat="server" ID="scriptManager" Theme="Gray" />
	
	<ext:ViewPort runat="server">
		<Body>
			<ext:FitLayout runat="server">
				<ext:FormPanel runat="server" ID="pnlForm" BodyStyle="padding:5px;">
					<TopBar>
						<ext:Toolbar runat="server">
							<Items>
								<ext:Button runat="server" ID="btnSave" Text="Save" Icon="PageSave" OnClick="btnSave_Click" AutoPostBack="true" />
								<ext:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cross" OnClick="btnCancel_Click" AutoPostBack="true" CausesValidation="false" />
							</Items>
						</ext:Toolbar>
					</TopBar>
					<Body>
						<ext:FormLayout runat="server">
							<ext:Anchor>
								<ext:Checkbox runat="server" ID="chkEnableCache" FieldLabel="Enable page cache?" LabelSeparator="" />
							</ext:Anchor>
							<ext:Anchor>
								<ext:TimeField runat="server" ID="tmeCacheDuration" FieldLabel="Cache duration" SelectedTime="01:00:00" Width="80" />
							</ext:Anchor>
						</ext:FormLayout>
					</Body>
					<BottomBar>
						<ext:Toolbar runat="server">
							<Items>
								<ext:Button runat="server" ID="btnSave2" Text="Save" Icon="PageSave" OnClick="btnSave_Click" AutoPostBack="true" />
								<ext:Button runat="server" ID="btnCancel2" Text="Cancel" Icon="Cross" OnClick="btnCancel_Click" AutoPostBack="true" CausesValidation="false" />
							</Items>
						</ext:Toolbar>
					</BottomBar>
				</ext:FormPanel>
			</ext:FitLayout>
		</Body>
	</ext:ViewPort>
</asp:Content>