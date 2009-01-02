<%@ Page Language="C#" MasterPageFile="../MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="FreeGiftProduct.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.FreeGiftProduct" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1><%= this.CurrentItem.Title %></h1> 
			
	<div id="productImage">
		<zeus:ItemDetailView runat="server" PropertyName="Image" />
	</div>
	
	<div id="productDetails">
		<p><%= this.CurrentItem.Description %></p>
	</div>
</asp:Content>