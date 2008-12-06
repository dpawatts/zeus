<%@ Page Title="Add New Item" Language="C#" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="Zeus.Admin.New" MasterPageFile="PreviewFrame.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<p>Please choose the type of item you would like to add.</p>
	
	<asp:ListView runat="server" ID="lsvChildTypes">
		<LayoutTemplate>
			<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
		</LayoutTemplate>
		<ItemTemplate>
			<p>
				<a href="edit.aspx?discriminator=<%# Eval("Discriminator") %>&parentid=<%# Request.QueryString["parentid"] %>"><img runat="server" src='<%# Eval("IconUrl") %>' alt="" /></a>
				<a href="edit.aspx?discriminator=<%# Eval("Discriminator") %>&parentid=<%# Request.QueryString["parentid"] %>"><%# Eval("DefinitionAttribute.Title") %></a>
			</p>
		</ItemTemplate>
		<EmptyDataTemplate>
			<p>You cannot add an item below this location.</p>
		</EmptyDataTemplate>
	</asp:ListView>
</asp:Content>