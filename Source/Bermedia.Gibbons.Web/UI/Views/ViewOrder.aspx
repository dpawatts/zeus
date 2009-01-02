<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="ViewOrder.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.ViewOrder" %>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>View Order #<%= this.CurrentItem.ID %></h1>

	<table width="695" border="0" cellspacing="0" cellpadding="0">
		<tr>
			<td width="60%" valign="top" class="dottedTopBorder">
				<strong>Order Placed:</strong> <%= this.CurrentItem.DatePlaced.ToLongDateString() %><br />
				<strong>Order Number:</strong> <%= this.CurrentItem.ID %><br />
				<strong>Order Status:</strong> <%= this.CurrentItem.StatusDescription %><br />
				<strong>Delivery Method:</strong> <%= this.CurrentItem.DeliveryType.Title %>
			</td>
			<td valign="top" class="dottedTopBorder">
				<asp:PlaceHolder runat="server" Visible="<%$ Code:this.CurrentItem.DeliveryType.RequiresShippingAddress %>">
					<strong>Shipping Address:</strong><br />
					<%= this.Customer.FullName %><br />
					<%= this.CurrentItem.ShippingAddress.AddressLine1%><br />
					<%= this.CurrentItem.ShippingAddress.AddressLine2%><br />
					<%= this.CurrentItem.ShippingAddress.City%><br />
					<%= this.CurrentItem.ShippingAddress.ParishState%><br />
					<%= this.CurrentItem.ShippingAddress.Zip%><br />
					<%= this.CurrentItem.ShippingAddress.Country.Title%><br />
					<strong>Phone: </strong><%= this.CurrentItem.ShippingAddress.PhoneNumber%><br />
					<br />
				</asp:PlaceHolder>
			</td>
		</tr>
		<tr>
			<td valign="top" class="dottedTopBorder">
				<strong>Shipping Cost:</strong> <%= this.CurrentItem.DeliveryType.GetPrice(this.CurrentItem).ToString("C2") %><br />
				<strong>Item Cost Total:</strong> <%= this.CurrentItem.ItemTotalPrice.ToString("C2") %><br />
				<strong>Total Cost:</strong> <%= this.CurrentItem.TotalPrice.ToString("C2") %><br />
			</td>
			<td valign="top" class="dottedTopBorder">
				<strong>Billing Address:</strong><br />
				<%= this.CurrentItem.BillingAddress.AddressLine1%><br />
				<%= this.CurrentItem.BillingAddress.AddressLine2%><br />
				<%= this.CurrentItem.BillingAddress.City%><br />
				<%= this.CurrentItem.BillingAddress.ParishState%><br />
				<%= this.CurrentItem.BillingAddress.Zip%><br />
				<%= this.CurrentItem.BillingAddress.Country.Title%><br />
				<strong>Phone: </strong><%= this.CurrentItem.BillingAddress.PhoneNumber%><br />
			</td>
		</tr>
		<tr>
			<td colspan="2" valign="top" class="dottedTopBorder">&nbsp;</td>
		</tr>
	</table>
	
	<h2>Items Ordered</h2>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="basket">
		<tr>
			<th>Item</th>
			<th>Size</th>
			<th>Colour</th>
			<th>Qty</th>
			<th>Gift Wrap</th>
			<th align="right">Cost</th>
		</tr>
		<isis:TypedListView runat="server" ID="lsvShoppingCartItems" DataItemTypeName="Bermedia.Gibbons.Web.Items.OrderItem, Bermedia.Gibbons.Web">
			<LayoutTemplate>
				<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
			</LayoutTemplate>
			<ItemTemplate>
				<tr class="<%# (Container.DataItem.Refunded) ? "refunded" : string.Empty %>">
					<td><%# Container.DataItem.Product.Title %></td>
					<td><%# (Container.DataItem.Size != null) ? Container.DataItem.Size.Title : string.Empty %></td>
					<td><%# (Container.DataItem.Colour != null) ? Container.DataItem.Colour.Title : string.Empty %></td>
					<td><%# Container.DataItem.Quantity %></td>
					<td><%# (Container.DataItem.GiftWrapType != null)? Container.DataItem.GiftWrapType.Title : string.Empty %></td>
					<td align="right"><%# Container.DataItem.Price.ToString("C2") %></td>
				</tr>
			</ItemTemplate>
		</isis:TypedListView>
		<tr>
			<td colspan="5" align="right">Shipping Cost</td>
			<td align="right"><%= this.CurrentItem.DeliveryType.GetPrice(this.CurrentItem).ToString("C2")%></td>
		</tr>
		<tr>
			<td colspan="5" align="right"><strong>TOTAL</strong></td>
			<td align="right"><strong><%= this.CurrentItem.TotalPrice.ToString("C2")%></strong></td>
		</tr>
	</table>
</asp:Content>
