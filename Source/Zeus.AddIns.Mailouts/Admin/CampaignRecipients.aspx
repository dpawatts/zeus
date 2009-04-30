<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CampaignRecipients.aspx.cs" Inherits="Zeus.AddIns.Mailouts.Admin.CampaignRecipients" %>
<%@ Import Namespace="Zeus.AddIns.Mailouts.Services"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link rel="stylesheet" href="/assets/css/shared.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<h2>Campaign Recipients</h2>
	
	<table>
		<tr>
			<th>Email</th>
		</tr>
		<% foreach (IMailoutRecipient recipient in GetRecipients()) { %>
		<tr>
			<td><%= recipient.Email %></td>
		</tr>
		<% } %>
	</table>
</asp:Content>