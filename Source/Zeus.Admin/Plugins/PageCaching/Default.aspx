<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.PageCaching.Default" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<ext:ResourceManager runat="server" ID="scriptManager" Theme="Gray" />
	
	<ext:Viewport runat="server">
		<Content>
			<ext:FitLayout runat="server">
				<Items>
					<ext:FormPanel runat="server" ID="pnlForm" BodyStyle="padding:5px;">
						<TopBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button runat="server" ID="btnSave" Text="Save" Icon="PageSave" OnClick="btnSave_Click" AutoPostBack="true" />
									<ext:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cross" OnClick="btnCancel_Click" AutoPostBack="true" CausesValidation="false" />
								</Items>
							</ext:Toolbar>
						</TopBar>
						<Content>
							<ext:FormLayout runat="server">
								<Anchors>
									<ext:Anchor>
										<ext:Checkbox runat="server" ID="chkEnableCache" FieldLabel="Enable page cache?" LabelSeparator="" />
									</ext:Anchor>
									<ext:Anchor>
										<ext:TimeField runat="server" ID="tmeCacheDuration" FieldLabel="Cache duration" SelectedTime="01:00:00" Width="80" />
									</ext:Anchor>
								</Anchors>
							</ext:FormLayout>
						</Content>
						<BottomBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button runat="server" ID="btnSave2" Text="Save" Icon="PageSave" OnClick="btnSave_Click" AutoPostBack="true" />
									<ext:Button runat="server" ID="btnCancel2" Text="Cancel" Icon="Cross" OnClick="btnCancel_Click" AutoPostBack="true" CausesValidation="false" />
								</Items>
							</ext:Toolbar>
						</BottomBar>
					</ext:FormPanel>
				</Items>
			</ext:FitLayout>
		</Content>
	</ext:Viewport>
</asp:Content>