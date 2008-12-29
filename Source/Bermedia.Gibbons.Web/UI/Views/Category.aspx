<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.Category" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<%@ Register TagPrefix="gibbons" TagName="ProductListing" Src="../UserControls/ProductListing.ascx" %>
<asp:Content ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1><zeus:ItemDetailView runat="server" PropertyName="Title" /></h1>

	<div runat="server" id="categoryImage" visible="<%$ HasValue:Image %>">
		<zeus:ItemDetailView runat="server" PropertyName="Image" />
	</div>
	
	<gibbons:ProductListing runat="server" ID="uscProductListing1" />
	<gibbons:ProductListing runat="server" ID="uscProductListing2" />
</asp:Content>