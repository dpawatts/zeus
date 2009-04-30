<%@ Page Title="Delete Item" Language="C#" AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="Zeus.Admin.Delete" %>
<%@ Register TagPrefix="isis" Namespace="Isis.Web.UI.HtmlControls" Assembly="Isis" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register Src="AffectedItems.ascx" TagName="AffectedItems" TagPrefix="zeus" %>
<%@ Register Src="ReferencingItems.ascx" TagName="ReferencingItems" TagPrefix="zeus" %>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<admin:ToolbarButton runat="server" ID="btnDelete" OnClick="btnDelete_Click" Text="Delete" ImageResourceName="Zeus.Admin.Assets.Images.Icons.delete.png" CssClass="negative" />
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" ImageResourceName="Zeus.Admin.Assets.Images.Icons.cross.png" CssClass="negative" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<asp:CustomValidator ID="csvDelete" runat="server" CssClass="validator info" Display="Dynamic" />
	<asp:CustomValidator ID="csvException" runat="server" CssClass="validator info" Display="Dynamic" />
	<table>
		<tr>
			<td>
				<isis:FieldSet runat="server" class="affectedItems" Legend="Affected items">
					<zeus:AffectedItems id="uscItemsToDelete" runat="server" />
				</isis:FieldSet>
			</td>
			<td>
				<isis:FieldSet runat="server" class="referencingItems" Legend="Items referencing the items you're deleting">
					<zeus:ReferencingItems id="uscReferencingItems" runat="server" />
				</isis:FieldSet>
			</td>
		</tr>
	</table>
</asp:Content>