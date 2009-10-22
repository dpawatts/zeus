<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.Versions.Default" %>
<%@ Import Namespace="Zeus.BaseLibrary.Web.UI"%>
<%@ Import Namespace="Zeus.Admin.Plugins.Versions"%>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Web.UI.WebControls" Assembly="Zeus" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" ImageResourceName="Zeus.Admin.Assets.Images.Icons.cross.png" CssClass="negative" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<asp:CustomValidator ID="cvVersionable" runat="server" Text="This item is not versionable." CssClass="info validator" Display="Dynamic" />
	
	<p><asp:CheckBox runat="server" ID="chkShowAllLanguages" Text=" Show all languages" AutoPostBack="true" OnCheckedChanged="chkShowAllLanguages_CheckedChanged" /></p>
	<br />
	
	<div id="grid">
		<asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="false" DataKeyNames="ID" CssClass="tb"
			UseAccessibleHeader="true" BorderWidth="0" OnRowCommand="gvHistory_RowCommand" OnRowDeleting="gvHistory_RowDeleting"
			HeaderStyle-CssClass="titles">
			<Columns>
				<asp:TemplateField HeaderText="Version" ItemStyle-CssClass="Version">
					<ItemTemplate><%# Eval("Version") %></ItemTemplate>
				</asp:TemplateField>
				<zeus:TemplateField HeaderText="Language" Visible='<%# GlobalizationEnabled %>'>
					<ItemTemplate><%# GetLanguage(Eval("Language") as string) %></ItemTemplate>
				</zeus:TemplateField>
				<asp:TemplateField HeaderText="Title">
					<ItemTemplate>
						<a href="<%# GetPreviewUrl((Zeus.ContentItem)Container.DataItem) %>" title="<%# Eval("ID") %>"><img alt="icon" src='<%# Eval("IconUrl") %>'/><%# string.IsNullOrEmpty(((Zeus.ContentItem)Container.DataItem).Title) ? "(untitled)" : ((Zeus.ContentItem)Container.DataItem).Title %></a>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Status">
					<ItemTemplate><%# GetStatus((Zeus.ContentItem)Container.DataItem) %></ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField HeaderText="Published" DataField="Published" meta:resourceKey="published" />
				<asp:BoundField HeaderText="Expired" DataField="Expires" meta:resourceKey="expires" />
				<asp:BoundField HeaderText="Saved by" DataField="SavedBy" meta:resourceKey="savedBy" />
				<asp:TemplateField HeaderText="Edit" ItemStyle-Width="50">
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="hlEdit" NavigateUrl='<%# Engine.AdminManager.GetEditExistingItemUrl((Zeus.ContentItem) Container.DataItem) %>'><asp:Image runat="server" ImageUrl='<%# WebResourceUtility.GetUrl(typeof(Default), "Zeus.Admin.Resources.page_edit.png") %>' AlternateText="Edit" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Publish" ItemStyle-Width="50">
					<ItemTemplate>
						<asp:ImageButton runat="server" ID="btnPublish" AlternateText="Publish" CommandName="Publish" CommandArgument='<%# Eval("ID") %>' Visible="<%# !IsPublished(Container.DataItem) %>" ImageUrl='<%# WebResourceUtility.GetUrl(typeof(Default), "Zeus.Admin.Assets.Images.Icons.book_next.png") %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Delete" ItemStyle-Width="50">
					<ItemTemplate>
						<asp:ImageButton runat="server" ID="btnDelete" AlternateText="Delete" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' Visible="<%# !IsPublished(Container.DataItem) %>" ImageUrl='<%# WebResourceUtility.GetUrl(typeof(Default), "Zeus.Admin.Resources.page_delete.png") %>' />
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
	</div>
</asp:Content>