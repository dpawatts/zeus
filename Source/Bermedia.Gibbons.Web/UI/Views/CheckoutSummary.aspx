<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CheckoutSummary.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.CheckoutSummary" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<table width="100%" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td width="400"><h1>Order Summary</h1></td>
			<td width="*" align="right" style="vertical-align:top">
				<sitdap:DynamicImageButton runat="server" ID="btnNext1" TemplateName="Button" AlternateText="place order" OnClick="btnNext_Click">
					<Layers>
						<sitdap:TextLayer Name="Text" Text="place order" />
					</Layers>
				</sitdap:DynamicImageButton>
			</td>
		</tr>
	</table>
	
	<table width="695" border="0" cellspacing="0" cellpadding="0">
		<tr>
			<td width="60%" valign="top">
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td><strong>Delivery Method:</strong></td>
						<td><%= this.ShoppingCart.DeliveryType.Title %></td>
					</tr>
					<tr>
						<td><strong>Item Cost Total:</strong></td>
						<td><%= this.ShoppingCart.ItemTotalPrice.ToString("C2") %></td>
					</tr>
					<tr>
						<td><strong>Shipping Cost:</strong></td>
						<td><%= this.ShoppingCart.DeliveryType.GetPrice(this.ShoppingCart).ToString("C2") %></td>
					</tr>
					<tr>
						<td><strong>Total Cost:</strong></td>
						<td><%= this.ShoppingCart.TotalPrice.ToString("C2") %></td>
					</tr>
					<tr>
						<td colspan="2">
							<br />
							<strong>Payment Details</strong><br />
							<%= this.CheckoutData.CardDetails %><br />
							<a href="checkout-payment-details.aspx?return=true">Change Payment Details</a>
						</td>
					</tr>
				</table>
			</td>
			<td valign="top" class="dottedLeftBorder">
				<asp:PlaceHolder runat="server" Visible="<%$ Code:this.ShoppingCart.DeliveryType.RequiresShippingAddress %>">
					<strong>Shipping Address:</strong><br />
					<%= this.Customer.FullName %><br />
					<%= this.ShoppingCart.ShippingAddress.AddressLine1 %><br />
					<%= this.ShoppingCart.ShippingAddress.AddressLine2 %><br />
					<%= this.ShoppingCart.ShippingAddress.City %><br />
					<%= this.ShoppingCart.ShippingAddress.ParishState %><br />
					<%= this.ShoppingCart.ShippingAddress.Zip %><br />
					<%= this.ShoppingCart.ShippingAddress.Country.Title %><br />
					<strong>Phone: </strong><%= this.ShoppingCart.ShippingAddress.PhoneNumber %><br />
					<a href="checkout-shipping-address.aspx?return=true">Change Address</a><br /><br />
				</asp:PlaceHolder>
				
				<strong>Billing Address:</strong><br />
				<%= this.ShoppingCart.BillingAddress.AddressLine1 %><br />
				<%= this.ShoppingCart.BillingAddress.AddressLine2 %><br />
				<%= this.ShoppingCart.BillingAddress.City %><br />
				<%= this.ShoppingCart.BillingAddress.ParishState %><br />
				<%= this.ShoppingCart.BillingAddress.Zip %><br />
				<%= this.ShoppingCart.BillingAddress.Country.Title %><br />
				<strong>Phone: </strong><%= this.ShoppingCart.BillingAddress.PhoneNumber %><br />
				<a href="checkout-billing-address.aspx?return=true">Change Address</a><br /><br />
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
			<th></th>
		</tr>
		<isis:TypedListView runat="server" ID="lsvShoppingCartItems" DataItemTypeName="Bermedia.Gibbons.Web.Items.ShoppingCartItem, Bermedia.Gibbons.Web">
			<LayoutTemplate>
				<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
			</LayoutTemplate>
			<ItemTemplate>
				<tr>
					<td><%# Container.DataItem.Product.Title %></td>
					<td><%# (Container.DataItem.Size != null) ? Container.DataItem.Size.Title : string.Empty %></td>
					<td><%# (Container.DataItem.Colour != null) ? Container.DataItem.Colour.Title : string.Empty %></td>
					<td><%# Container.DataItem.Quantity %></td>
					<td><%# (Container.DataItem.GiftWrapType != null)? Container.DataItem.GiftWrapType.Title : string.Empty %></td>
					<td align="right"><%# Container.DataItem.Price.ToString("C2") %></td>
					<td><a href="checkout-change-quantities.aspx">Change Quantities</a></td>
				</tr>
			</ItemTemplate>
		</isis:TypedListView>
		<tr>
			<td colspan="5" align="right">Shipping Cost</td>
			<td align="right"><%= this.ShoppingCart.DeliveryType.GetPrice(this.ShoppingCart).ToString("C2") %></td>
		</tr>
		<tr>
			<td colspan="5" align="right"><strong>TOTAL</strong></td>
			<td align="right"><strong><%= this.ShoppingCart.TotalPrice.ToString("C2") %></strong></td>
		</tr>
	</table>
	
	<p align="right">
		<sitdap:DynamicImageButton runat="server" ID="btnNext2" TemplateName="Button" AlternateText="place order" OnClick="btnNext_Click">
			<Layers>
				<sitdap:TextLayer Name="Text" Text="place order" />
			</Layers>
		</sitdap:DynamicImageButton>
	</p>
</asp:Content>
