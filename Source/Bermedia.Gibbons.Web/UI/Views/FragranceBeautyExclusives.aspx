<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="FragranceBeautyExclusives.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.FragranceBeautyExclusives" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<%@ Register TagPrefix="gibbons" TagName="ProductListing" Src="../UserControls/ProductListing.ascx" %>
<asp:Content ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Gibbons' Exclusives</h1>

	<gibbons:ProductListing runat="server" ID="uscProductListing" />
</asp:Content>