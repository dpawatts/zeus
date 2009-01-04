<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="GiftCardPage.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.GiftCardPage" %>
<%@ Register TagPrefix="gibbons" TagName="PageNavigation" Src="../UserControls/PageNavigation.ascx" %>
<asp:Content ContentPlaceHolderID="cphStyle" runat="server">
	#contentContainer {
		padding: 0;
	}
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:PageNavigation runat="server" />
	<%= this.CurrentItem.NavigationText %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<%= this.CurrentItem.Text %>
</asp:Content>