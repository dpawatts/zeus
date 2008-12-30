<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="FragranceBeautyScents.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.FragranceBeautyScents" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<%@ Register TagPrefix="gibbons" TagName="ProductListing" Src="../UserControls/ProductListing.ascx" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Bermedia.Gibbons.Web.Items" %>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Fragrances By Scent</h1>
	
	<asp:Repeater runat="server" ID="rptScents">
		<ItemTemplate>
			<h2 style="color:#<%# Eval("Key.HexRef") %>"><%# Eval("Key.Title") %></h2>
			<p><%# Eval("Key.Description") %></p>
			
			<gibbons:ProductListing runat="server" DataSource='<%# ((System.Linq.IGrouping<ProductScent, FragranceBeautyProduct>) Container.DataItem).Cast<StandardProduct>() %>' />
		</ItemTemplate>
	</asp:Repeater>
</asp:Content>