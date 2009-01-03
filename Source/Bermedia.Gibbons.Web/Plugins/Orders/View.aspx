<%@ Page Language="C#" MasterPageFile="~/Admin/PreviewFrame.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Bermedia.Gibbons.Web.Plugins.Orders.View" %>
<asp:Content ContentPlaceHolderID="Head" runat="server">
	<link rel="stylesheet" href="/admin/assets/css/shared.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	<link rel="stylesheet" href="/admin/assets/css/view.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	
	<script type="text/javascript" src="/admin/assets/js/jquery.js"></script>
	<script type="text/javascript" src="/admin/assets/js/zeus.js"></script>
	<script type="text/javascript" src="/admin/assets/js/view.js"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<h2>Orders</h2>
	
	<div id="filterBar">
		<asp:Label runat="server" AssociatedControlID="ddlFilterByDeliveryType">Filter by Delivery Type</asp:Label>
		<asp:DropDownList runat="server" ID="ddlFilterByDeliveryType" AutoPostBack="true" DataSourceID="cdsDeliveryTypes" DataTextField="Title" DataValueField="ID" AppendDataBoundItems="true">
			<asp:ListItem Value="-1">[None]</asp:ListItem>
		</asp:DropDownList>
		<zeus:ContentDataSource runat="server" ID="cdsDeliveryTypes" Axis="Descendant" Query="RootItem" OfType="Bermedia.Gibbons.Web.Items.BaseDeliveryType" />
		
		<asp:Label runat="server" AssociatedControlID="ddlFilterByStatus">Filter by Status</asp:Label>
		<asp:DropDownList runat="server" ID="ddlFilterByStatus" AutoPostBack="true">
			<asp:ListItem Value="-1">[None]</asp:ListItem>
			<asp:ListItem Value="1">New</asp:ListItem>
			<asp:ListItem Value="2">Collected</asp:ListItem>
			<asp:ListItem Value="3">Shipped</asp:ListItem>
			<asp:ListItem Value="4">Deleted</asp:ListItem>
		</asp:DropDownList>
	</div>
	
	<asp:UpdatePanel runat="server" ID="updEntities">
		<ContentTemplate>
			<isis:TypedListView runat="server" DataSourceID="cdsChildren" DataItemTypeName="Bermedia.Gibbons.Web.Items.Order, Bermedia.Gibbons.Web">
				<LayoutTemplate>
					<div class="gridToolbar">
						<div class="pageNo">
							<asp:DataPager runat="server" ID="dpgEntities1" PageSize="25">
								<Fields>
									<isis:GooglePagerField NextPageImageUrl="~/Admin/Assets/Images/View/button_arrow_right.gif"
										PreviousPageImageUrl="~/Admin/Assets/Images/View/button_arrow_left.gif" />
								</Fields>
							</asp:DataPager>
						</div>
					</div><div class="divide">
						<!-- -->
					</div>
					<table runat="server" id="dataTable" class="tb">
						<tr class="titles">
							<th class="data"><asp:LinkButton runat="server" ID="lnkSortDatePlaced" Text="Date" CommandName="Sort" CommandArgument="DatePlaced" /></th>
							<th class="data"><asp:LinkButton runat="server" ID="lnkSortCustomerName" Text="Customer Name" CommandName="Sort" CommandArgument="Customer.FullName" /></th>
							<th class="data"><asp:LinkButton runat="server" ID="lnkSortStatus" Text="Status" CommandName="Sort" CommandArgument="Status" /></th>
							<th class="data"><asp:LinkButton runat="server" ID="lnkSortDeliveryType" Text="Delivery Type" CommandName="Sort" CommandArgument="DeliveryType" /></th>
							<th class="data"><asp:LinkButton runat="server" ID="lnkSortOrderNumber" Text="Order Number" CommandName="Sort" CommandArgument="ID" /></th>
							<th class="edit"><!-- --></th>
						</tr>
						<tr runat="server" ID="itemPlaceholder" />
					</table>
					<div class="divide">
						<!-- -->
					</div><div class="gridToolbar">
						<div class="pageNo">
							<asp:DataPager runat="server" ID="dpgEntities2" PageSize="25">
								<Fields>
									<isis:GooglePagerField NextPageImageUrl="~/Admin/Assets/Images/View/button_arrow_right.gif"
										PreviousPageImageUrl="~/Admin/Assets/Images/View/button_arrow_left.gif" />
								</Fields>
							</asp:DataPager>
						</div>
					</div>
				</LayoutTemplate>
				<ItemTemplate>
					<tr>
						<td><%# Container.DataItem.DatePlaced %></td>
						<td><%# Container.DataItem.Customer.FullName %></td>
						<td><%# Container.DataItem.StatusDescription %></td>
						<td><%# Container.DataItem.DeliveryType.Title %></td>
						<td><%# Container.DataItem.ID %></td>
						<td class="edit">
							<a href="Edit.aspx?Selected=<%# Server.UrlEncode(Container.DataItem.Path) %>" class="edit">Edit</a>
						</td>
					</tr>
				</ItemTemplate>
				<EmptyDataTemplate>
					<p>There are no orders of this type.</p>
				</EmptyDataTemplate>
			</isis:TypedListView>
		</ContentTemplate>
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="ddlFilterByDeliveryType" EventName="SelectedIndexChanged" />
			<asp:AsyncPostBackTrigger ControlID="ddlFilterByStatus" EventName="SelectedIndexChanged" />
		</Triggers>
	</asp:UpdatePanel>
	<zeus:ContentDataSource runat="server" ID="cdsChildren" Axis="Descendant" Query="RootItem" OfType="Bermedia.Gibbons.Web.Items.Order" Where="(Status != @BasketStatus) && (@DeliveryType == -1 || DeliveryType.ID == @DeliveryType) && (@Status == -1 || Int32(Status) == @Status)">
		<WhereParameters>
			<isis:ExtendedParameter Name="BasketStatus" EnumType="Bermedia.Gibbons.Web.Items.OrderStatus" Value="Basket" />
			<asp:ControlParameter ControlID="ddlFilterByDeliveryType" Name="DeliveryType" Type="Int32" PropertyName="SelectedValue" />
			<asp:ControlParameter ControlID="ddlFilterByStatus" Name="Status" Type="Int32" PropertyName="SelectedValue" />
		</WhereParameters>
	</zeus:ContentDataSource>
</asp:Content>