<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReferencingItems.ascx.cs" Inherits="Zeus.Admin.ReferencingItems" %>
<asp:Repeater runat="server" ID="rptItems">
	<HeaderTemplate><ul></HeaderTemplate>
	<ItemTemplate>
		<li><a runat="server" href='<%# Eval("Url") %>'><asp:Image runat="server" ImageUrl='<%# Eval("IconUrl") %>' AlternateText='<%# Eval("Name") %>' /><%# Eval("Title") %></a></li>
	</ItemTemplate>
	<FooterTemplate></ul></FooterTemplate>
</asp:Repeater>