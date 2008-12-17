<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="DepartmentGifts.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.DepartmentGifts" %>
<%@ Register TagPrefix="gibbons" TagName="ProductListing" Src="../UserControls/ProductListing.ascx" %>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Gifts Under $<%= Request.QueryString["PriceLimit"] %></h1>
	
	<gibbons:ProductListing runat="server" DataSourceID="cdsGifts" />
	<zeus:ChildrenDataSource runat="server" ID="cdsGifts" OfType="Bermedia.Gibbons.Web.Items.StandardProduct"
		Where="RegularPrice <= @PriceLimit && GiftItem" Axis="Descendant">
		<WhereParameters>
			<asp:QueryStringParameter Name="PriceLimit" QueryStringField="PriceLimit" Type="Decimal" />
		</WhereParameters>
	</zeus:ChildrenDataSource>
</asp:Content>