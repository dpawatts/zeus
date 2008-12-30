<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="NonFragranceBeautyCategory.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.NonFragranceBeautyCategory" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<%@ Register TagPrefix="gibbons" TagName="ProductListing" Src="../UserControls/ProductListing.ascx" %>
<asp:Content ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1><zeus:ItemDetailView runat="server" PropertyName="Title" /></h1>

	<gibbons:ProductListing runat="server" ID="uscProductListing" />
</asp:Content>