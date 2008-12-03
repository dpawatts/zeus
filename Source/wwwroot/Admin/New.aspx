<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="Zeus.Edit.New" MasterPageFile="PreviewFrame.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<asp:Repeater runat="server" ID="rptItemDefinitions">
		<ItemTemplate>
			<p><a href="edit.aspx?discriminator=<%# Eval("Discriminator") %>"><%# Eval("DefinitionAttribute.Title") %></a></p>
		</ItemTemplate>
	</asp:Repeater>
</asp:Content>