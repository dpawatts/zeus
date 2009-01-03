<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.MyAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSubNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1>My Account</h1>

  <h2>My Orders</h2>

  <ul>
		<li><a href="<%= new Zeus.Web.Url(this.CurrentItem.Url).AppendSegment("track-orders") %>">Track orders</a></li>
  </ul>
  
  <h2>Account Settings</h2>
  <ul>
		<li><a href="<%= new Zeus.Web.Url(this.CurrentItem.Url).AppendSegment("personal-details") %>">Change name, email address or password</a></li>
    <li><a href="<%= new Zeus.Web.Url(this.CurrentItem.Url).AppendSegment("manage-address-book") %>">Manage address book</a></li>
    <li><a href="#">Subscribe/Unsubscribe from mailing list</a></li>
  </ul>
</asp:Content>
