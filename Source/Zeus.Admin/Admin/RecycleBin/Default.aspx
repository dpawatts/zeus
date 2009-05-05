<%@ Page Title="Recycle Bin" Language="C#" MasterPageFile="~/Admin/PreviewFrame.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.RecycleBin.Default" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register TagPrefix="isis" Namespace="Isis.Web.UI.WebControls" Assembly="Isis" %>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Web.UI.WebControls" Assembly="Zeus" %>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
	<admin:ToolbarButton runat="server" ID="btnEmpty" Text="Empty Recycle Bin" ImageResourceName="Zeus.Admin.Assets.Images.Icons.bin_empty.png" CssClass="negative" OnClick="btnEmpty_Click" />
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" ImageResourceName="Zeus.Admin.Assets.Images.Icons.cross.png" CssClass="negative" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<asp:CustomValidator ID="cvRestore" CssClass="validator" ErrorMessage="An item with the same name already exists at the previous location." runat="server" Display="Dynamic" />
	<zeus:ContentDataSource id="cdsRecycleBinItems" runat="server" />
	<isis:TypedGridView ID="grvRecycleBinItems" DataItemTypeName="Zeus.ContentItem, Zeus" DataKeyNames="ID" runat="server" DataSourceID="cdsRecycleBinItems" AutoGenerateColumns="false" OnRowCommand="grvRecycleBinItems_RowCommand" EmptyDataText="No items in recycle bin." CssClass="gv" AlternatingRowStyle-CssClass="alt">
		<Columns>
			<isis:TypedTemplateField HeaderText="Title">
				<ItemTemplate>
					<asp:HyperLink runat="server" NavigateUrl='<%# Container.DataItem.Url %>'>
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
					<asp:HyperLink ID="hlPreviousLocation" runat="server" NavigateUrl='<%# ((Zeus.ContentItem) Container.DataItem["FormerParent"]).Url %>'>
						<asp:Image ID="Image2" runat="server" ImageUrl='<%#((Zeus.ContentItem) Container.DataItem["FormerParent"]).IconUrl %>' />
						<%# ((Zeus.ContentItem) Container.DataItem["FormerParent"]).Title %>
					</asp:HyperLink>
				</ItemTemplate>
			</isis:TypedTemplateField>
			<asp:ButtonField Text="Restore" CommandName="Restore" meta:resourceKey="colRestore" />
			<asp:ButtonField Text="Delete" CommandName="Delete" meta:resourceKey="colDelete" />
		</Columns>
	</isis:TypedGridView>
</asp:Content>
