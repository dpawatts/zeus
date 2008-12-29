<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Page.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.Page" %>
<%@ Register TagPrefix="gibbons" TagName="PageNavigation" Src="../UserControls/PageNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:PageNavigation runat="server" />
	<%= this.CurrentItem.NavigationText %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<%= this.CurrentItem.Text %>
</asp:Content>