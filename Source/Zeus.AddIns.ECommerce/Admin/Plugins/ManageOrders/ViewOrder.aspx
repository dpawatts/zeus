<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageOrders.aspx.cs" Inherits="Zeus.AddIns.ECommerce.Plugins.ViewOrder" %>
<%@ Import Namespace="Zeus.AddIns.ECommerce.ContentTypes.Data"%>
<%@ Import Namespace="Zeus.BaseLibrary.ExtensionMethods"%>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<admin:ToolbarButton runat="server" ID="btnProcess" Text="Process" Icon="BasketGo" CssClass="positive" OnClick="btnProcess_Click" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<h2>View Order</h2>

	<table class="tb">
		<tr class="titles">
			<th>Customer</th>
			<th>Billing Address</th>
			<th>Shipping Address</th>
		</tr>
		<tr>
			<td>
				Email: <%= SelectedOrder.EmailAddress %><br />
				Telephone: <%= SelectedOrder.TelephoneNumber %><br />
				Mobile: <%= SelectedOrder.MobileTelephoneNumber %>
			</td>
			<td>
				<%= SelectedOrder.BillingAddress.PersonTitle %> <%= SelectedOrder.BillingAddress.FirstName %> <%= SelectedOrder.BillingAddress.Surname %><br />
				<%= SelectedOrder.BillingAddress.AddressLine1 %><br />
				<%= SelectedOrder.BillingAddress.AddressLine2 %><br />
				<%= SelectedOrder.BillingAddress.TownCity %><br />
				<%= SelectedOrder.BillingAddress.Postcode %><br />
			</td>
			<td>
				<%= SelectedOrder.ShippingAddress.PersonTitle %> <%= SelectedOrder.ShippingAddress.FirstName %> <%= SelectedOrder.ShippingAddress.Surname %><br />
				<%= SelectedOrder.ShippingAddress.AddressLine1 %><br />
				<%= SelectedOrder.ShippingAddress.AddressLine2 %><br />
				<%= SelectedOrder.ShippingAddress.TownCity %><br />
				<%= SelectedOrder.ShippingAddress.Postcode %><br />
			</td>
		</tr>
	</table>
	
	<table class="tb">
		<tr class="titles">
			<th>Status</th>
			<th>Delivery Method</th>
			<th>Order Date</th>
			<th>Order Number</th>
		</tr>
		<tr>
			<td><%= SelectedOrder.Status.GetDescription() %></td>
			<td><%= SelectedOrder.DeliveryMethod.Title %></td>
			<td><%= SelectedOrder.Created %></td>
			<td><%= SelectedOrder.ID %></td>
		</tr>
	</table>
	
	<table class="tb">
		<tr class="titles">
			<th>Product</th>
			<th>Variations</th>
			<th>Quantity</th>
			<th>Price Per Unit</th>
			<th>Line Total</th>
		</tr>
		<% foreach (OrderItem orderItem in SelectedOrder.Items) { %>
		<tr>
			<td><%= orderItem.ProductTitle %></td>
			<td>
				<% foreach (string variation in orderItem.Variations) { %>
				<%= variation %><br />
				<% } %>
			</td>
			<td><%= orderItem.Quantity %></td>
			<td><%= orderItem.Price.ToString("C2")%></td>
			<td><%= orderItem.LineTotal.ToString("C2")%></td>
		</tr>
		<% } %>
		<tr>
			<td colspan="2">Delivery Price</td>
			<td>1</td>
			<td><%= SelectedOrder.DeliveryPrice.ToString("C2") %></td>
			<td><%= SelectedOrder.DeliveryPrice.ToString("C2") %></td>
		</tr>
		<tr>
			<td colspan="4">TOTAL</td>
			<td><%= SelectedOrder.TotalPrice.ToString("C2") %></td>
		</tr>
	</table>
</asp:Content>