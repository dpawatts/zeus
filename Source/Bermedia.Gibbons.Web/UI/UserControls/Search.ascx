<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="Bermedia.Gibbons.Web.UI.UserControls.Search" %>

<asp:Panel runat="server" DefaultButton="btnSearch">
	<asp:Label runat="server" AssociatedControlID="txtSearchText"><strong>Search</strong></asp:Label><br />
	<asp:TextBox runat="server" ID="txtSearchText" MaxLength="50" CssClass="searchMe" /><br />
	<asp:DropDownList runat="server" ID="ddlSearchDepartment" CssClass="searchMe">
		<asp:ListItem Value="">All Departments</asp:ListItem>
	</asp:DropDownList><br />
	<asp:Button runat="server" ID="btnSearch" Text="Go" CssClass="mySubmit" OnClick="btnSearch_Click" />
</asp:Panel>