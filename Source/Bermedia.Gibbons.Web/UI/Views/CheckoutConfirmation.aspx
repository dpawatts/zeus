<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CheckoutConfirmation.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.CheckoutConfirmation" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1>Order Confirmation</h1>
	
	<asp:Label runat="server" ID="lblConfirmationText" />
	<br /><br />
	
	<h2 runat="server" id="h2SuccessMessage">Thank you for shopping with Gibbons Company.</h2>
</asp:Content>
