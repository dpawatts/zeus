﻿<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="DepartmentGifts.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.DepartmentGifts" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<%@ Register TagPrefix="gibbons" TagName="ProductListing" Src="../UserControls/ProductListing.ascx" %>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Gifts Under $<%= Request.QueryString["PriceLimit"] %></h1>
	
	<gibbons:ProductListing runat="server" ID="uscProductListing" />
</asp:Content>