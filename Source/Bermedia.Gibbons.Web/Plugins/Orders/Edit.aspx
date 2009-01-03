<%@ Page Language="C#" MasterPageFile="~/Admin/PreviewFrame.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Bermedia.Gibbons.Web.Plugins.Orders.Edit" %>
<%@ Import Namespace="Isis" %>
<asp:Content ContentPlaceHolderID="Head" runat="server">
	<link rel="stylesheet" href="/admin/assets/css/shared.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	<link rel="stylesheet" href="/admin/assets/css/view.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	
	<style type="text/css">
		tr.refunded td
		{
			background-color: #FFCCCC;
		}
	</style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<asp:Button runat="server" ID="btnCollect" Visible="false" OnClick="btnCollect_Click" Text="Collect (processes payment)" /> 
	<asp:Button runat="server" ID="btnShip" Visible="false" OnClick="btnShip_Click" Text="Ship (processes payment)" /> 
	<asp:Button runat="server" ID="btnDeleteRefund" Visible="false" OnClick="btnDeleteRefund_Click" Text="Delete (refunds payment)" /> 
	<asp:Button runat="server" ID="btnDeleteCancel" Visible="false" OnClick="btnDeleteCancel_Click" Text="Delete (cancels payment)" /> 
	<asp:Button runat="server" ID="btnRefundSelected" Visible="false" OnClick="btnRefundSelected_Click" Text="Refund Selected Items" /> 
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<h2 id="itemTitle">View Order</h2>
	<p><asp:Label runat="server" ID="lblError" CssClass="error" /></p>
	<isis:TypedExtendedFormView runat="server" ID="fmvEntity" DataItemTypeName="Bermedia.Gibbons.Web.Items.Order, Bermedia.Gibbons.Web" DefaultMode="Edit">
		<InsertEditItemTemplate>
			<table class="tb">
				<tr class="titles">
					<th style="width:33%">Customer</th>
					<th style="width:33%">Billing Address</th>
					<th style="width:33%">Shipping Address</th>
				</tr>
				<tr>
					<td style="vertical-align:top">
						<span class="labelDescription">Name:</span>
						<%# Container.DataItem.Customer.FullName %><br />
						<span class="labelDescription">E-Mail:</span>
						<%# Container.DataItem.Customer.Email %>
					</td>
					<isis:ConditionalMultiView runat="server" Value='<%# Container.DataItem.BillingAddress %>'>
						<isis:ConditionalView runat="server" Expression="it != null">
							<ItemTemplate>
								<td style="vertical-align:top">
									<%# Eval("AddressLine1") %><br />
									<%# Eval("AddressLine2") %><br />
									<%# Eval("City") %><br />
									<%# Eval("ParishState") %><br />
									<%# Eval("Zip") %><br />
									<%# Eval("Country.Name") %><br /><br />
									<span class="labelDescription">Telephone:</span>
									<%# Eval("PhoneNumber") %>
								</td>
							</ItemTemplate>
						</isis:ConditionalView>
					</isis:ConditionalMultiView>
					<isis:ConditionalMultiView runat="server" Value='<%# Container.DataItem.ShippingAddress %>'>
						<isis:ConditionalView runat="server" Expression="it != null">
							<ItemTemplate>
								<td style="vertical-align:top">
									<%# Eval("AddressLine1") %><br />
									<%# Eval("AddressLine2") %><br />
									<%# Eval("City") %><br />
									<%# Eval("ParishState") %><br />
									<%# Eval("Zip") %><br />
									<%# Eval("Country.Name") %><br /><br />
									<span class="labelDescription">Telephone:</span>
									<%# Eval("PhoneNumber") %>
								</td>
							</ItemTemplate>
						</isis:ConditionalView>
					</isis:ConditionalMultiView>
				</tr>
			</table>
				
			<table class="tb">
				<tr class="titles">
					<th style="width:14%">Status</th>
					<th style="width:14%">Payment</th>
					<th style="width:14%">Delivery Type</th>
					<th style="width:14%">Order Date</th>
					<th style="width:14%">Order Number</th>
					<th style="width:30%">Tracking Number</th>
				</tr>
				<tr>			
					<td style="text-align:center"><%# Container.DataItem.Status.GetDescription() %></td>
					<td style="text-align:center"><%# Container.DataItem.PaymentStatus.GetDescription() %></td>
					<td style="text-align:center"><%# Container.DataItem.DeliveryType.Title %></td>
					<td style="text-align:center"><%# Container.DataItem.DatePlaced %></td>
					<td style="text-align:center"><%# Container.DataItem.ID %></td>
					<td style="text-align:center">
						<asp:TextBox runat="server" ID="txtTrackingNumber" Text='<%# Container.DataItem.TrackingNumber %>' Width="70" />
						<asp:Button runat="server" ID="btnUpdateTrackingNumber" Text="Update" OnClick="btnUpdateTrackingNumber_Click" />
					</td>
				</tr>
			</table>

			<table class="tb">
				<tr class="titles">
					<th style="width:5%">Cancel / Refund</th>
					<th style="width:35%">Product</th>
					<th style="width:10%">Gift Wrap</th>
					<th style="width:10%">Gift Receipt Req.</th>
					<th style="width:10%">Quantity Ordered</th>
					<th style="width:10%">Purchase Price Per Unit</th>
					<th style="width:10%">Purchase Price</th>
				</tr>
				<isis:TypedRepeater runat="server" ID="rptOrderedProducts" DataSource='<%# Container.DataItem.GetChildren<Bermedia.Gibbons.Web.Items.OrderItem>() %>' DataItemTypeName="Bermedia.Gibbons.Web.Items.OrderItem, Bermedia.Gibbons.Web">
					<ItemTemplate>
						<tr class="<%# Container.DataItem.Refunded ? "refunded" : string.Empty %>">
							<td>
								<asp:HiddenField runat="server" ID="hdnOrderItemID" Value='<%# Container.DataItem.ID %>' />
								<asp:CheckBox runat="server" ID="chkCancel" Visible='<%# !Container.DataItem.Refunded && HasOrderBeenPaidFor() %>' />
							</td>
							<td>
								<%# Container.DataItem.Product.VendorStyleNumber + "&nbsp;&nbsp;" + Container.DataItem.Product.Title %>
								<%# Container.DataItem.Product.FreeGiftProduct.GetValueOrDefault(fgp => "(Free Gift: " + fgp.Title + ")", string.Empty) %>
							</td>
							<td><%# Container.DataItem.GiftWrapType.GetValueOrDefault(gwt => gwt.Title, string.Empty) %></td>
							<td><%# (Container.DataItem.IsGift) ? "Yes" : "No" %></td>
							<td style="text-align:right"><%# Container.DataItem.Quantity %></td>
							<td style="text-align:right"><%# Container.DataItem.PricePerUnit.ToString("C2") %></td>
							<td style="text-align:right"><%# Container.DataItem.Price.ToString("C2") %></td>
						</tr>
					</ItemTemplate>
				</isis:TypedRepeater>
				<tr>
					<td></td>
					<td>Delivery Charge</td>
					<td></td>
					<td></td>
					<td style="text-align:right">1</td>
					<td style="text-align:right"><%# Container.DataItem.DeliveryPrice.ToString("C2") %></td>
					<td style="text-align:right"><%# Container.DataItem.DeliveryPrice.ToString("C2")%></td>
				</tr>
				<tr>
					<td colspan="7" style="text-align:right"><span class="labelDescription"><%# Container.DataItem.TotalPrice.ToString("C2") %></span></td>
				</tr>
			</table>
		</InsertEditItemTemplate>
	</isis:TypedExtendedFormView>
</asp:Content>