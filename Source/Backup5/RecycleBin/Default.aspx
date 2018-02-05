<%@ Page Title="Recycle Bin" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.RecycleBin.Default" %>
<%@ Import Namespace="Ext.Net" %>
<%@ Import Namespace="Zeus.BaseLibrary.Web.UI"%>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Web.UI.WebControls" Assembly="Zeus" %>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
	<admin:ToolbarButton runat="server" ID="btnEmpty" Text="Empty Recycle Bin" Icon="BinEmpty" CssClass="negative" OnClick="btnEmpty_Click" />
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" Icon="Cross" CssClass="negative" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<asp:CustomValidator ID="cvRestore" CssClass="validator" ErrorMessage="An item with the same name already exists at the previous location." runat="server" Display="Dynamic" />
	<zeus:TypedGridView ID="grvRecycleBinItems" DataItemTypeName="Zeus.ContentItem, Zeus" DataKeyNames="ID" runat="server" AutoGenerateColumns="false"
		OnRowCommand="grvRecycleBinItems_RowCommand" OnRowDeleting="grvRecycleBinItems_RowDeleting" EmptyDataText="No items in recycle bin." CssClass="tb"
		HeaderStyle-CssClass="titles" AlternatingRowStyle-CssClass="alt">
		<Columns>
			<zeus:TypedTemplateField HeaderText="Title">
				<ItemTemplate>
					<asp:HyperLink runat="server" NavigateUrl='<%# ((Zeus.INode) Container.DataItem).PreviewUrl %>'>
						<asp:Image runat="server" ImageUrl='<%# Container.DataItem.IconUrl %>' />
						<%# Container.DataItem.Title %>
					</asp:HyperLink>
				</ItemTemplate>
			</zeus:TypedTemplateField>
			<zeus:TypedTemplateField HeaderText="Deleted">
				<ItemTemplate>
					<%# Container.DataItem["DeletedDate"] %>				
				</ItemTemplate>
			</zeus:TypedTemplateField>
			<zeus:TypedTemplateField HeaderText="Previous location">
				<ItemTemplate>
					<asp:HyperLink ID="hlPreviousLocation" runat="server" NavigateUrl='<%# ((Zeus.INode) Container.DataItem["FormerParent"]).PreviewUrl %>'>
						<asp:Image ID="Image2" runat="server" ImageUrl='<%#((Zeus.ContentItem) Container.DataItem["FormerParent"]).IconUrl %>' />
						<%# ((Zeus.ContentItem) Container.DataItem["FormerParent"]).Title %>
					</asp:HyperLink>
				</ItemTemplate>
			</zeus:TypedTemplateField>
			<asp:TemplateField HeaderText="Restore" ItemStyle-Width="50">
					<ItemTemplate>
						<asp:ImageButton runat="server" ID="btnRestore" AlternateText="Restore" CommandName="Restore" CommandArgument='<%# Eval("ID") %>'
							ImageUrl='<%# Zeus.Utility.GetCooliteIconUrl(Icon.ArrowRedo)) %>' />
					</ItemTemplate>
				</asp:TemplateField>
			<asp:TemplateField HeaderText="Delete" ItemStyle-Width="50">
				<ItemTemplate>
					<asp:ImageButton runat="server" ID="btnDelete" AlternateText="Delete" CommandName="Delete" CommandArgument='<%# Eval("ID") %>'
						ImageUrl='<%# Zeus.Utility.GetCooliteIconUrl(Icon.PageDelete) %>' />
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</zeus:TypedGridView>
</asp:Content>
