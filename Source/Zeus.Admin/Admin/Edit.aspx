<%@ Page Language="C#" MasterPageFile="~/Admin/PreviewFrame.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Zeus.Admin.Edit" ValidateRequest="false" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register TagPrefix="zeus" TagName="AvailableZones" Src="~/Admin/AvailableZones.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<asp:Panel runat="server" ID="pnlPageView" CssClass="rightAligned">
		<asp:PlaceHolder runat="server" ID="plcLanguages" Visible="false">
			<b>Page view:</b> <admin:LanguageDropDownList runat="server" ID="ddlLanguages" OnLanguageChanged="ddlLanguages_LanguageChanged" />
		</asp:PlaceHolder>
		<asp:HyperLink runat="server" ID="hplZones" CssClass="showZones command" NavigateUrl="javascript:void(0);">Zones</asp:HyperLink>
	</asp:Panel>
	<admin:ToolbarButton runat="server" ID="btnSave" Text="Save and publish" ImageResourceName="Zeus.Admin.Assets.Images.Icons.page_save.png" CssClass="positive" OnClick="btnSave_Click" />
	<admin:ToolbarButton runat="server" ID="btnSaveUnpublished" Text="Save an unpublished version" ImageResourceName="Zeus.Admin.Assets.Images.Icons.book_next.png" CssClass="positive" OnClick="btnSaveUnpublished_Click" />
	<admin:ToolbarButton runat="server" ID="btnPreview" Text="Save and preview" ImageResourceName="Zeus.Admin.Assets.Images.Icons.zoom.png" CssClass="positive" OnClick="btnPreview_Click" />
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" ImageResourceName="Zeus.Admin.Assets.Images.Icons.cross.png" CssClass="negative" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Outside">
	<div class="right">
		<zeus:AvailableZones runat="server" ID="uscZones" />
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<asp:HyperLink ID="hlNewerVersion" runat="server" Text="There is a newer version of this item that hasn't been published. Edit the newer version &amp;raquo;" CssClass="info" Visible="false" />
	<asp:HyperLink ID="hlOlderVersion" runat="server" Text="This is a version of another item that is the master version. Edit the master version &amp;raquo;" CssClass="info" Visible="false" />
	
	<asp:ValidationSummary runat="server" CssClass="info validator" />
	<asp:CustomValidator ID="csvException" runat="server" Display="None" />
	
	<zeus:ItemEditView runat="server" ID="zeusItemEditView" OnItemCreating="zeusItemEditView_ItemCreating"
		OnDefinitionCreating="zeusItemEditView_DefinitionCreating" OnSaving="zeusItemEditView_Saving" />
	
	<script type="text/javascript">
		jQuery(document).ready(function() {
			jQuery(".right fieldset").hide();

			jQuery(".showInfo").toggle(function() {
				zeustoggle.show(this, ".infoBox");
			}, function() {
				zeustoggle.hide(this, ".infoBox");
			});

			jQuery(".showZones").toggle(function() {
				zeustoggle.show(this, ".zonesBox");
			}, function() {
				zeustoggle.hide(this, ".zonesBox");
			});

			if (cookie.read(".infoBox"))
				jQuery(".showInfo").click();
			if (cookie.read(".zonesBox"))
				jQuery(".showZones").click();
		});
   </script>
</asp:Content>