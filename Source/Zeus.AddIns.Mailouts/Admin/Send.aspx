<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Send.aspx.cs" Inherits="Zeus.AddIns.Mailouts.Admin.Send" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<admin:ToolbarButton runat="server" ID="btnSend" Text="Send" Icon="EmailGo" CssClass="positive" OnClick="btnSend_Click" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<h2>Send "<%= SelectedItem.Title %>" Campaign</h2>
	
	<p><b>This campaign will be delivered to <a href="<%= GetRecipientsUrl() %>" target="_blank" onclick="window.open(this.href, 'recipients', 'width=300,height=500');return false;"><%= GetRecipientCount() %> recipient(s)</a>.</b></p>
</asp:Content>