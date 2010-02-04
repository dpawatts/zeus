<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.AddIns.ECommerce.Admin.Plugins.ManageOrders.Default" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Web.UI.WebControls" Assembly="Zeus" %>
<%@ Import Namespace="Zeus.BaseLibrary.ExtensionMethods" %>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<h2>Manage Orders</h2>

	<zeus:TypedListView runat="server" ID="lsvOrders" DataItemTypeName="Zeus.AddIns.ECommerce.ContentTypes.Data.Order"
		OnPagePropertiesChanging="lsvOrders_PagePropertiesChanging">
		<LayoutTemplate>
			<table class="tb">
				<tr class="titles">
					<th>Order Number</th>
					<th>Date Placed</th>
					<th>Customer</th>
					<th># Items</th>
					<th>Total Price</th>
					<th>Status</th>
					<th>Details</th>
				</tr>
				<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
			</table>
		</LayoutTemplate>
		<ItemTemplate>
			<tr>
				<td style="vertical-align:top"><%# Container.DataItem.ID %></td>
				<td style="vertical-align:top"><%# Container.DataItem.Created.ToShortDateString() %></td>
				<td style="vertical-align:top"><%# Container.DataItem.EmailAddress%></td>
				<td style="vertical-align:top"><%# Container.DataItem.TotalItemCount%></td>
				<td style="vertical-align:top"><%# Container.DataItem.TotalPrice.ToString("C2") %></td>
				<td style="vertical-align:top"><%# Container.DataItem.Status.GetDescription() %></td>
				<td style="vertical-align:top">
					<a href="vieworder.aspx?selected=<%# Container.DataItem.Path %>">Details</a>
				</td>
			</tr>
		</ItemTemplate>
	</zeus:TypedListView>
	
	<asp:DataPager ID="dpgSearchResultsPager" runat="server" PageSize="30" PagedControlID="lsvOrders">
		<Fields>
      <asp:NumericPagerField ButtonCount="10" ButtonType="Link" />
    </Fields>
	</asp:DataPager>
</asp:Content>