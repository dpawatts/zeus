<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.Globalization.DeleteLanguage.Default" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
	<admin:ToolbarButton runat="server" ID="btnDelete" OnClick="btnDelete_Click" Text="Delete" Icon="Delete" CssClass="negative" />
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" Icon="Cross" CssClass="negative" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<p>Delete one of the languages for the page, without deleting the entire page. <!-- Note! When you delete a language for a page, 
	it will be permanently deleted. This means that it will not be moved to the Recycle Bin. -->Any sub-pages in the same language will not be deleted.</p>
	<asp:CustomValidator ID="csvDelete" runat="server" CssClass="validator info" Display="Dynamic" />
	<asp:CustomValidator ID="csvException" runat="server" CssClass="validator info" Display="Dynamic" />

	<div class="editDetail">
		<asp:Label runat="server" AssociatedControlID="ddlLanguages" CssClass="editorLabel">Delete Language</asp:Label>
		<asp:DropDownList runat="server" ID="ddlLanguages" Width="200" AppendDataBoundItems="true" DataValueField="Name" DataTextField="Title">
			<asp:ListItem Value="">[Select language to delete]</asp:ListItem>
		</asp:DropDownList>
		<asp:RequiredFieldValidator runat="server" ControlToValidate="ddlLanguages" ErrorMessage="You must select a language" />
	</div>
</asp:Content>
