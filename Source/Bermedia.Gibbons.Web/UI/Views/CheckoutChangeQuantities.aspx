<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CheckoutChangeQuantities.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.CheckoutChangeQuantities" %>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Change Quantities</h1>
	
	<p>You can change any of the quantities of the products you are ordering. <strong>Setting the quantity to zero (0) will remove the item from your order.</strong></p>
	
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
		<isis:TypedListView runat="server" ID="lsvShoppingCartItems" DataKeyNames="ID" DataItemTypeName="Bermedia.Gibbons.Web.Items.ShoppingCartItem, Bermedia.Gibbons.Web">
			<LayoutTemplate>
				<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
			</LayoutTemplate>
			<ItemTemplate>
				<tr>
					<td><%# Container.DataItem.Product.Title %></td>
					<td><%# (Container.DataItem.Size != null) ? Container.DataItem.Size.Title : string.Empty %></td>
					<td><%# (Container.DataItem.Colour != null) ? Container.DataItem.Colour.Title : string.Empty %></td>
					<td><asp:TextBox runat="server" ID="txtQuantity" Width="30px" Text='<%# Container.DataItem.Quantity %>' /></td>
					<td><%# (Container.DataItem.GiftWrapType != null)? Container.DataItem.GiftWrapType.Title : string.Empty %></td>
					<td align="right"><%# Container.DataItem.Price.ToString("C2") %></td>
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
		<sitdap:DynamicImageButton runat="server" ID="btnUpdateQuantities" TemplateName="Button" AlternateText="update quantities" OnClick="btnUpdateQuantities_Click">
			<Layers>
				<sitdap:TextLayer Name="Text" Text="update quantities" />
			</Layers>
		</sitdap:DynamicImageButton>
	</p>
</asp:Content>
