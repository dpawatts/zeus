<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="FragranceBeautyBrand.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.FragranceBeautyBrand" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<%@ Register TagPrefix="gibbons" TagName="ProductListing" Src="../UserControls/ProductListing.ascx" %>
<asp:Content ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1><asp:Literal runat="server" ID="ltlTitle" /></h1>

	<gibbons:ProductListing runat="server" ID="uscProductListing" />
</asp:Content>