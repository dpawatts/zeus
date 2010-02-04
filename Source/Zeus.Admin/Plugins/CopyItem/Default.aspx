<%@ Page Title="Move Item" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.CopyItem.Default" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register Src="/admin/AffectedItems.ascx" TagName="AffectedItems" TagPrefix="zeus" %>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<admin:ToolbarButton runat="server" ID="btnCopy" Text="Try Again" Icon="PageCopy" CssClass="positive" OnClick="btnCopy_Click" />
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" Icon="Cross" CssClass="negative" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<asp:CustomValidator id="cvCopy" meta:resourceKey="cvCopy" runat="server" CssClass="validator info" />
	<asp:CustomValidator ID="cvException" runat="server" CssClass="validator info" Display="Dynamic" />

	<asp:Panel runat="server" ID="pnlNewName" CssClass="editDetail" Visible="false">
		<asp:Label runat="server" ID="lblNewName" AssociatedControlID="txtNewName" Text="New name" CssClass="editorLabel" />
		<asp:TextBox ID="txtNewName" runat="server" CssClass="textEditor" />
	</asp:Panel>
	<div class="editDetail">
		<asp:Label ID="lblFrom" runat="server" AssociatedControlID="from" Text="From" CssClass="editorLabel" />
		<asp:Label ID="from" runat="server"/>
	</div>
	<div class="editDetail">
		<asp:Label ID="lblTo" runat="server" AssociatedControlID="to" Text="To" CssClass="editorLabel" />
		<asp:Label ID="to" runat="server"/>
	</div>
	<hr />
	<h3 id="h3" runat="server">Copied items:</h3>
	<div class="affectedItems">
		<zeus:AffectedItems id="itemsToCopy" runat="server" />
	</div>
</asp:Content>