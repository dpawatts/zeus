<%@ Page Language="C#" MasterPageFile="~/Admin/PreviewFrame.Master" AutoEventWireup="true" CodeBehind="ShippingRates.aspx.cs" Inherits="Bermedia.Gibbons.Web.Plugins.ShippingRates" %>
<asp:Content ContentPlaceHolderID="Head" runat="server">
	<link rel="stylesheet" href="/admin/assets/css/view.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
</asp:Content>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
	<asp:Button runat="server" ID="btnSave" CssClass="save" Text="Save" OnClick="btnSave_Click" /> 
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<h2>International Shipping Rates</h2>
	
	<asp:Table runat="server" ID="tblShippingRates" CssClass="tb" />
</asp:Content>
