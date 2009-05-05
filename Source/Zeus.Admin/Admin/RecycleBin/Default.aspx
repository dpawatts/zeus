<%@ Page Title="Recycle Bin" Language="C#" MasterPageFile="~/Admin/PreviewFrame.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.RecycleBin.Default" %>
<%@ Import Namespace="Isis.Web.UI"%>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register TagPrefix="isis" Namespace="Isis.Web.UI.WebControls" Assembly="Isis" %>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Web.UI.WebControls" Assembly="Zeus" %>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
	<admin:ToolbarButton runat="server" ID="btnEmpty" Text="Empty Recycle Bin" ImageResourceName="Zeus.Admin.Assets.Images.Icons.bin_empty.png" CssClass="negative" OnClick="btnEmpty_Click" />
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" ImageResourceName="Zeus.Admin.Assets.Images.Icons.cross.png" CssClass="negative" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<asp:CustomValidator ID="cvRestore" CssClass="validator" ErrorMessage="An item with the same name already exists at the previous location." runat="server" Display="Dynamic" />
	<isis:TypedGridView ID="grvRecycleBinItems" DataItemTypeName="Zeus.ContentItem, Zeus" DataKeyNames="ID" runat="server" AutoGenerateColumns="false"
		OnRowCommand="grvRecycleBinItems_RowCommand" OnRowDeleting="grvRecycleBinItems_RowDeleting" EmptyDataText="No items in recycle bin." CssClass="tb"
		HeaderStyle-CssClass="titles" AlternatingRowStyle-CssClass="alt">
		<Columns>
			<isis:TypedTemplateField HeaderText="Title">
				<ItemTemplate>
					<asp:HyperLink runat="server" NavigateUrl='<%# ((Zeus.INode) Container.DataItem).PreviewUrl %>'>
						<asp:Image runat="server" ImageUrl='<%# Container.DataItem.IconUrl %>' />
						<%# Container.DataItem.Title %>
					</asp:HyperLink>
				</ItemTemplate>
			</isis:TypedTemplateField>
			<isis:TypedTemplateField HeaderText="Deleted">
				<ItemTemplate>
					<%# Container.DataItem["DeletedDate"] %>				
				</ItemTemplate>
			</isis:TypedTemplateField>
			<isis:TypedTemplateField HeaderText="Previous location">
				<ItemTemplate>
					<asp:HyperLink ID="hlPreviousLocation" runat="server" NavigateUrl='<%# ((Zeus.INode) Container.DataItem["FormerParent"]).PreviewUrl %>'>
						<asp:Image ID="Image2" runat="server" ImageUrl='<%#((Zeus.ContentItem) Container.DataItem["FormerParent"]).IconUrl %>' />
						<%# ((Zeus.ContentItem) Container.DataItem["FormerParent"]).Title %>
					</asp:HyperLink>
				</ItemTemplate>
			</isis:TypedTemplateField>
			<asp:TemplateField HeaderText="Restore" ItemStyle-Width="50">
					<ItemTemplate>
						<asp:ImageButton runat="server" ID="btnRestore" AlternateText="Restore" CommandName="Restore" CommandArgument='<%# Eval("ID") %>'
							ImageUrl='<%# WebResourceUtility.GetUrl(typeof(Zeus.Admin.RecycleBin.Default), "Zeus.Admin.Assets.Images.Icons.arrow_redo.png") %>' />
					</ItemTemplate>
				</asp:TemplateField>
			<asp:TemplateField HeaderText="Delete" ItemStyle-Width="50">
				<ItemTemplate>
					<asp:ImageButton runat="server" ID="btnDelete" AlternateText="Delete" CommandName="Delete" CommandArgument='<%# Eval("ID") %>'
						ImageUrl='<%# WebResourceUtility.GetUrl(typeof(Zeus.Admin.RecycleBin.Default), "Zeus.Admin.Resources.page_delete.png") %>' />
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</isis:TypedGridView>
</asp:Content>
