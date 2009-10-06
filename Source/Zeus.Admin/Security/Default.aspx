<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Security.Default" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
	<admin:ToolbarButton runat="server" ID="btnSave" Text="Save" ImageResourceName="Zeus.Admin.Assets.Images.Icons.tick.png" CssClass="positive" OnClick="btnSave_Click" />
	<admin:ToolbarButton runat="server" ID="btnSaveRecursive" Text="Save Whole Branch" ImageResourceName="Zeus.Admin.Assets.Images.Icons.tick.png" CssClass="positive" OnClick="btnSaveRecursive_Click" />
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" ImageResourceName="Zeus.Admin.Assets.Images.Icons.cross.png" CssClass="negative" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<asp:Table runat="server" ID="tblPermissions" CssClass="permissions" />
	
	<br />
	<h3>Add New Role or User</h3>
	<div class="editDetail">
		<asp:Label runat="server" AssociatedControlID="ddlRoles" CssClass="editorLabel">Role</asp:Label>
		<asp:DropDownList runat="server" ID="ddlRoles" Width="200" AppendDataBoundItems="true">
			<asp:ListItem></asp:ListItem>
		</asp:DropDownList>
		<asp:Button runat="server" ID="btnAddRole" Text="Add" OnClick="btnAddRole_Click" /><br />
	</div>
	<div class="editDetail">
		<asp:Label runat="server" AssociatedControlID="ddlUsers" CssClass="editorLabel">User</asp:Label>
		<asp:DropDownList runat="server" ID="ddlUsers" Width="200" AppendDataBoundItems="true">
			<asp:ListItem></asp:ListItem>
		</asp:DropDownList>
		<asp:Button runat="server" ID="btnAddUser" Text="Add" OnClick="btnAddUser_Click" /><br />
	</div>
</asp:Content>
