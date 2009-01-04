<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="TrackOrders.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.TrackOrders" %>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>My Orders</h1>

	<isis:TypedListView runat="server" ID="lsvOrders" DataItemTypeName="Bermedia.Gibbons.Web.Items.Order, Bermedia.Gibbons.Web">
		<LayoutTemplate>
			<table width="715" border="0" cellspacing="0" cellpadding="0">
				<tr>
					<th width="100" valign="top">Order Date</th>
					<th width="70" valign="top">Order #</th>
					<th width="250" valign="top">Items</th>
					<th width="70" valign="top">Total</th>
					<th width="70" valign="top">
						Tracking<br />
						Number
					 </th>
					<th width="160" valign="top">Order Status</th>
					<th valign="top">&nbsp;</th>
				</tr>
				<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
			</table>
		</LayoutTemplate>
		<ItemTemplate>
			<tr>
				<td valign="top"><%# Container.DataItem.DatePlaced.ToShortDateString() %></td>
				<td valign="top"><%# Container.DataItem.ID %></td>
				<td valign="top">
					<ul>
						<isis:TypedRepeater runat="server" DataSource='<%# Container.DataItem.GetChildren<Bermedia.Gibbons.Web.Items.BaseOrderItem>() %>' DataItemTypeName="Bermedia.Gibbons.Web.Items.BaseOrderItem, Bermedia.Gibbons.Web">
							<ItemTemplate>
								<li><%# Container.DataItem.Quantity %> x <%# Container.DataItem.ProductTitle %></li>
							</ItemTemplate>
						</isis:TypedRepeater>
					</ul>
				</td>
				<td valign="top"><%# Container.DataItem.TotalPrice.ToString("C2") %></td>
				<td valign="top"><%# Container.DataItem.TrackingNumber %></td>
				<td valign="top"><%# Container.DataItem.StatusDescription %></td>
				<td valign="top"><a href="<%# new Zeus.Web.Url(this.CurrentItem.Url).AppendSegment("view-order").AppendQuery("page", Container.DataItem.ID) %>".aspx">View Order</a></td>
			</tr>
		</ItemTemplate>
		<EmptyDataTemplate>
			<p>You do not currently have any orders.</p>
		</EmptyDataTemplate>
	</isis:TypedListView>
</asp:Content>
