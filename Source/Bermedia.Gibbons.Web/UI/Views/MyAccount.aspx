<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.MyAccount" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
	<link rel="stylesheet" href="/assets/css/myaccount.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
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
    <% if (this.Customer.NewsletterSubscription != null) { %>
    <li><a href="<%= new Zeus.Web.Url(this.CurrentItem.Url).AppendSegment("unsubscribe") %>">Unsubscribe from mailing list</a></li>
    <% } else { %>
    <li><a href="<%= new Zeus.Web.Url(this.CurrentItem.Url).AppendSegment("subscribe") %>">Subscribe to mailing list</a></li>
    <% } %>
  </ul>
</asp:Content>
