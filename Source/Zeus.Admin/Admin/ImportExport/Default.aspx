<%@ Page Title="Import / Export Items" Language="C#" MasterPageFile="~/Admin/PreviewFrame.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.ImportExport.Default" %>
<%@ Import Namespace="Isis.Web.UI"%>
<%@ Register Src="../AffectedItems.ascx" TagName="AffectedItems" TagPrefix="admin" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Web.UI.WebControls" Assembly="Zeus" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		p { margin-bottom: 10px; }
	</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" ImageResourceName="Zeus.Admin.Assets.Images.Icons.cross.png" CssClass="negative" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<zeus:TabControl runat="server" ID="tbcTabs">
		<zeus:TabItem runat="server" ID="tbpExport" ToolTip="Export">
			<p><asp:Button ID="btnExport" runat="server" CssClass="command" OnCommand="btnExport_Command" CausesValidation="false" Text="Export these items" /></p>
			<p><asp:CheckBox ID="chkDefinedDetails" runat="server" Text="Exclude computer generated data" /></p>
			
			<p><b>Exported Items</b></p>
			<div class="affectedItems">
				<admin:AffectedItems id="exportedItems" runat="server" />
			</div>
		</zeus:TabItem>
	
    <zeus:TabItem runat="server" ID="tbcImport" ToolTip="Import">
			<asp:CustomValidator id="cvImport" runat="server" CssClass="info validator" Display="Dynamic"/>
			<asp:MultiView ID="uploadFlow" runat="server" ActiveViewIndex="0">
				<asp:View ID="uploadView" runat="server">
					<div class="upload">
						<p>
							<asp:FileUpload ID="fuImport" runat="server" />
							<asp:RequiredFieldValidator ID="rfvUpload" ControlToValidate="fuImport" runat="server" ErrorMessage="*"  meta:resourceKey="rfvImport"/>
						</p>
						<p>
							<asp:Button ID="btnVerify" runat="server" Text="Upload and examine" OnClick="btnVerify_Click" Display="Dynamic" />
							<asp:Button ID="btnUploadImport" runat="server" Text="Import here" OnClick="btnUploadImport_Click" />
				    </p>
					</div>
				</asp:View>
				<asp:View ID="preView" runat="server">
					<p><asp:CheckBox ID="chkSkipRoot" runat="server" Text="Skip imported root item" ToolTip="Checking this options cause the first level item not to be imported, and its children to be added to the selected item's children" /></p>
					<asp:Button ID="btnImportUploaded" runat="server" Text="Import" OnClick="btnImportUploaded_Click" />
			    
					<p><b>Imported Items</b></p>
					<div class="affectedItems">
						<admin:AffectedItems id="importedItems" runat="server" />
					</div>
				</asp:View>
			</asp:MultiView>
		</zeus:TabItem>
	</zeus:TabControl>
</asp:Content>