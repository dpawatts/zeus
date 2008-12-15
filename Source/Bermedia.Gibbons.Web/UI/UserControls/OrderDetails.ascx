<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderDetails.ascx.cs" Inherits="Bermedia.Gibbons.Web.UI.UserControls.OrderDetails" %>
<%@ Import Namespace="Isis" %>
<asp:FormView runat="server" ID="fmvOrderDetails" DefaultMode="ReadOnly">
	<ItemTemplate>
		<table width="695" border="0" cellspacing="0" cellpadding="0">
			<tr>
				<td width="60%" valign="top">
					<table width="100%" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td width="150"><strong>Order Placed:</strong></td>
							<td><%# Eval("Date", "{0:dd MMMM yyyy}") %></td>
						</tr>
						<tr>
							<td><strong>Order Status:</strong></td>
							<td><%# ((SoundInTheory.BermyTix.Entities.Order) Container.DataItem).DeliveryMethod.GetDescription() %></td>
						</tr>
						<tr>
							<td><strong>Total Cost:</strong></td>
							<td>$<%# Eval("TotalPrice", "{0:F2}") %></td>
						</tr>
						<tr runat="server" visible='<%# ViewMode == SoundInTheory.BermyTix.Websites.Public.App_Shared.UserControls.OrderDetailsViewMode.CheckoutSummary %>'>
							<td><strong>Payment Details</strong></td>
							<td><%# Page.CheckoutData.CardDetails %></td>
						</tr>
					</table>
				</td>
				<td valign="top" class="dottedLeftBorder">
					<strong>Billing Address:</strong><br />
					<%# Eval("BillingAddress.Address1") %><br />
					<%# Eval("BillingAddress.Address2") %><br />
					<%# Eval("BillingAddress.City") %><br />
					<%# Eval("BillingAddress.Parish") %><br />
					<%# Eval("BillingAddress.ZIP") %><br />
					<%# Eval("BillingAddress.Country") %>
				</td>
			</tr>
			<tr>
				<td colspan="2" valign="top" class="dottedTopBorder">&nbsp;</td>
			</tr>
		</table>
		
		<h2>Items Ordered</h2>
		<table width="100%" border="0" cellspacing="0" cellpadding="0" class="basket">
			<tr>
				<th>Event</th>
				<th>Venue</th>
				<th>Date</th>
				<th>Time</th>
				<th>Seat #</th>
				<th>Type</th>
				<th align="right">Cost</th>
			</tr>
			<asp:Repeater runat="server" DataSource='<%# Eval("OrderedSeats") %>'>
				<ItemTemplate>
					<tr>
						<td><%# Eval("Performance.Name") %></td>
						<td><a runat="server" style="font-weight: normal;" onclick="return openVenuePopup(this.href);" href='<%# "~/venue-details.aspx?id=" + Eval("Performance.VenueID") %>'><%# Eval("Performance.Venue.Name") %></a></td>
						<td><%# Eval("Performance.Date", "{0:ddd. d MMM yyyy}" )%></td>
						<td><%# Eval("Performance.Time", "{0:h.mm tt}" )%></td>
						<td><%# Eval("SeatNumber") ?? "N/A" %></td>
						<td><%# Eval("TicketType.Name") %></td>
						<td align="right">$<%# Eval("Price", "{0:F2}") %></td>
					</tr>
				</ItemTemplate>
			</asp:Repeater>
			<tr>
				<td colspan="6" align="right"><strong>TOTAL</strong></td>
				<td align="right"><strong>$<%# Eval("TotalPrice", "{0:F2}") %></strong></td>
			</tr>
		</table>
	</ItemTemplate>
</asp:FormView>