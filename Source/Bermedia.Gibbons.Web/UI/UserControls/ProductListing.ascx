<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductListing.ascx.cs" Inherits="Bermedia.Gibbons.Web.UI.UserControls.ProductListing" %>
<%@ Register TagPrefix="gibbons" TagName="ProductRows" Src="ProductRows.ascx" %>

<h1 runat="server" id="h1Header" />

<asp:Label runat="server" ID="ltlPageLinks" style="float:right;height:20px;margin-bottom:10px;" />
<br style="clear:both" />

<asp:Panel runat="server" ID="pnlCategoryImage" />

<gibbons:ProductRows runat="server" ID="uscProductRows1" />
<gibbons:ProductRows runat="server" ID="uscProductRows2" />