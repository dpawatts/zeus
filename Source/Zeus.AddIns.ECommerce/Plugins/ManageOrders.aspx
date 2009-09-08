<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageOrders.aspx.cs" Inherits="Zeus.AddIns.ECommerce.Plugins.ManageOrders" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<h2>Manage Orders</h2>

	<isis:TypedListView runat="server" ID="lsvOrders" DataItemTypeName="Zeus.AddIns.ECommerce.ContentTypes.Data.Order">
		<LayoutTemplate>
			<table class="tb">
				<tr class="titles">
					<th>Date Placed</th>
					<th>Customer</th>
					<th># Items</th>
					<th>Total Price</th>
					<th>Details</th>
				</tr>
				<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
			</table>
		</LayoutTemplate>
		<ItemTemplate>
			<tr>
				<td style="vertical-align:top"><%# Container.DataItem.Created.ToShortDateString() %></td>
				<td style="vertical-align:top"><%# Container.DataItem.EmailAddress%></td>
				<td style="vertical-align:top"><%# Container.DataItem.TotalItemCount%></td>
				<td style="vertical-align:top"><%# Container.DataItem.TotalPrice.ToString("C2") %></td>
				<td style="vertical-align:top">
					<a href="vieworder.aspx?selected=<%# Container.DataItem.Path %>">Details</a>
				</td>
			</tr>
		</ItemTemplate>
	</isis:TypedListView>
</asp:Content>