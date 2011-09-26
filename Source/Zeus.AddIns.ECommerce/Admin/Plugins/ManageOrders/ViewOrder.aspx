<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewOrder.aspx.cs" Inherits="Zeus.AddIns.ECommerce.Plugins.ViewOrder" Debug="true"  %>
<%@ Import Namespace="Zeus.AddIns.ECommerce.ContentTypes.Data"%>
<%@ Import Namespace="Zeus.BaseLibrary.ExtensionMethods"%>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>

<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<admin:ToolbarButton runat="server" ID="btnProcess" Text="Process" Icon="BasketGo" CssClass="positive" OnClick="btnProcess_Click" />
	<admin:ToolbarButton runat="server" ID="btnCancel" Text="Cancel" Icon="Cross" CssClass="positive" OnClick="btnCancel_Click" />
	<admin:ToolbarButton runat="server" ID="btnBack" Text="Back to Manage Orders" Icon="ArrowLeft" CssClass="positive" OnClick="btnBack_Click" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<style>
/* ORDER TABLES */
            #adminTable, .tb {
                border-collapse: collapse; border-spacing: 0;
                margin: 0 10px 10px 0 ;
                float: left;
            }

            #adminTable td, #adminTable th, .tb td, .tb th{
                border: 1px solid #ddd;
                font-family: arial;
                padding: 7px;
            }

            #adminTable th, .tb th {
                font-weight: bold;
                color: Black;
                font-size: 12px;
                background: #f0fdff;
                text-align: left;
            }

            #adminTable td, .tb td {
                font-size: 12px;
                line-height: 17px;
            }
</style>

	<h2>View Order</h2>

    <table class="tb" id="adminTable">
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
	<div style="clear:both" />
	<table class="tb" id="adminTable">
		<tr class="titles">
			<th>Status</th>
			<th>Delivery Method</th>
			<th>Order Date</th>
			<th>Order Number</th>
		</tr>
		<tr>
			<td><%= SelectedOrder.Status.GetDescription() %></td>
			<td><%= SelectedOrder.DeliveryMethod == null ? "N/A" : SelectedOrder.DeliveryMethod.Title%></td>
			<td><%= SelectedOrder.Created %></td>
			<td><%= SelectedOrder.ID %></td>
		</tr>
	</table>
	<div style="clear:both" />
	
	<table class="tb" id="adminTable">
		<tr class="titles">
			<th>Product</th>
			
			<th>Quantity</th>
			<th>Price Per Unit</th>
			<th>Line Total</th>
		</tr>
		<% foreach (OrderItem orderItem in SelectedOrder.Items) { %>
		<tr>
			<td><%= orderItem.DisplayTitle %></td>
			
			<td><%= orderItem.Quantity %></td>
			<td><%= orderItem.Price.ToString("C2")%></td>
			<td><%= orderItem.LineTotal.ToString("C2")%></td>
		</tr>
		<% } %>
		
		<tr>
			<td colspan="3">TOTAL</td>
			<td><%= SelectedOrder.TotalPrice.ToString("C2") %></td>
		</tr>
	</table>
</asp:Content>