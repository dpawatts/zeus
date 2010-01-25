<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.EditItem.Default" ValidateRequest="false" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Web.UI.WebControls" Assembly="Zeus" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.WebControls" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" %>
<%@ Register TagPrefix="ext" Namespace="Coolite.Ext.UX" Assembly="Coolite.Ext.UX" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
	<ext:ResourcePlaceHolder runat="server" Mode="Script" />
	<ext:ResourcePlaceHolder runat="server" Mode="Style" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ToolbarContainer"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentContainer"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Outside">
	<ext:ResourceManager runat="server" Theme="Gray" />

	<ext:Viewport runat="server">
		<Content>
			<ext:BorderLayout runat="server">
				<Center>
					<ext:Panel runat="server" Border="false" BodyStyle="padding:5px" AutoScroll="true">
						<TopBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button runat="server" ID="btnSave" Text="Save and publish" Icon="PageSave" OnClick="btnSave_Click" OnClientClick="if (typeof(tinyMCE) !== 'undefined') { tinyMCE.triggerSave(false,true); } return true;" AutoPostBack="true" />
									<ext:Button runat="server" ID="btnSaveUnpublished" Text="Save an unpublished version" Icon="BookNext" OnClick="btnSaveUnpublished_Click" AutoPostBack="true" />
									<ext:Button runat="server" ID="btnPreview" Text="Preview" Icon="Zoom" OnClick="btnPreview_Click" AutoPostBack="true" />
									<ext:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cross" OnClick="btnCancel_Click" AutoPostBack="true" CausesValidation="false" />
									<ext:ToolbarFill runat="server" />
									<ext:ToolbarTextItem runat="server" ID="txiLanguages" Text="Page view: " />
									<ext:IconCombo runat="server" ID="ddlLanguages" Width="100" Editable="false" AutoPostBack="true" OnValueChanged="ddlLanguages_ValueChanged" />
								</Items>
							</ext:Toolbar>
						</TopBar>
						<Content>
							<asp:HyperLink ID="hlNewerVersion" runat="server" Text="There is a newer version of this item that hasn't been published. Edit the newer version &amp;raquo;" CssClass="info" Visible="false" />
							<asp:HyperLink ID="hlOlderVersion" runat="server" Text="This is a version of another item that is the master version. Edit the master version &amp;raquo;" CssClass="info" Visible="false" />
							
							<asp:ValidationSummary runat="server" CssClass="info validator" />
							<asp:CustomValidator ID="csvException" runat="server" Display="None" />
							
							<zeus:ItemEditView runat="server" ID="zeusItemEditView" OnItemCreating="zeusItemEditView_ItemCreating"
								OnDefinitionCreating="zeusItemEditView_DefinitionCreating" OnSaving="zeusItemEditView_Saving" />
						</Content>
						<BottomBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button runat="server" ID="btnSave2" Text="Save and publish" Icon="PageSave" OnClick="btnSave_Click" OnClientClick="if (typeof(tinyMCE) !== 'undefined') { tinyMCE.triggerSave(false,true); } return true;" AutoPostBack="true" />
									<ext:Button runat="server" ID="btnSaveUnpublished2" Text="Save an unpublished version" Icon="BookNext" OnClick="btnSaveUnpublished_Click" AutoPostBack="true" />
									<ext:Button runat="server" ID="btnPreview2" Text="Preview" Icon="Zoom" OnClick="btnPreview_Click" AutoPostBack="true" />
									<ext:Button runat="server" ID="btnCancel2" Text="Cancel" Icon="Cross" OnClick="btnCancel_Click" AutoPostBack="true" CausesValidation="false" />
									<ext:ToolbarFill runat="server" />
									<ext:ToolbarTextItem runat="server" ID="txiLanguages2" Text="Page view: " />
									<ext:IconCombo runat="server" ID="ddlLanguages2" Width="100" Editable="false" AutoPostBack="true" OnValueChanged="ddlLanguages_ValueChanged" />
								</Items>
							</ext:Toolbar>
						</BottomBar>
					</ext:Panel>
				</Center>
			</ext:BorderLayout>
		</Content>
	</ext:Viewport>
	
	<script type="text/javascript">
		Ext.onReady(function()
		{
			setTimeout(jQuery.zeusKeepAlive.sessionSaver,
				jQuery.zeusKeepAlive.sessionSaverInterval);

			window.top.zeus.setPreviewTitle('<%= Title.Replace("'", "\\'") %>');
		});

		(function($)
		{
			$.zeusKeepAlive =
			{
				sessionSaverUrl: '<%= GetSessionKeepAliveUrl() %>',
				sessionSaverInterval: (60000 * 5),
				sessionSaver: function()
				{
					$.post($.zeusKeepAlive.sessionSaverUrl);
					setTimeout($.zeusKeepAlive.sessionSaver,
						$.zeusKeepAlive.sessionSaverInterval);
				}
			};
		})(jQuery);

   </script>
</asp:Content>