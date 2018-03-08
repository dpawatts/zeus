<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Zeus.AddIns.ECommerce.Admin.Plugins.ManageOrders.Search" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Web.UI.WebControls" Assembly="Zeus" %>
<%@ Import Namespace="Zeus.BaseLibrary.ExtensionMethods" %>

<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
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
                font-size: 12px;
                color: Black;
                background: #f0fdff;
                text-align: left;
            }

            #adminTable td, .tb td {
                font-size: 12px;
                line-height: 17px;
            }
</style>
    <h2>Search for Orders</h2>
     <table>
        <tr>
            <td>Order Number</td>
            <td><asp:TextBox id="txtOrderNumber" runat="server" /></td>
        </tr>
        <tr>
            <td>Customer First Name</td>
            <td><asp:TextBox id="txtCustomerFirstName" runat="server" /></td>
        </tr>
        <tr>
            <td>Customer Last Name</td>
            <td><asp:TextBox id="txtCustomerLastName" runat="server" /></td>
        </tr>
        <tr>
            <td>Customer Email</td>
            <td><asp:TextBox id="txtCustomerEmail" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button runat="server" id="btnSearch" Text="Search" OnClick="btnSearch_Click" /></td>            
        </tr>
    </table>

	
	<zeus:TypedListView runat="server" ID="lsvOrders" DataItemTypeName="Zeus.AddIns.ECommerce.ContentTypes.Data.Order"
		OnPagePropertiesChanging="lsvOrders_PagePropertiesChanging">
		<LayoutTemplate>
			<table class="tb" id="adminTable">
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
				<td style="vertical-align:top"><%# Container.DataItem.ShippingAddress.FirstName + " " + Container.DataItem.ShippingAddress.Surname + " - " + Container.DataItem.EmailAddress%></td>
				<td style="vertical-align:top"><%# Container.DataItem.TotalItemCount%></td>
				<td style="vertical-align:top"><%# Container.DataItem.TotalPrice.ToString("C2") %></td>
				<td style="vertical-align:top"><%# Container.DataItem.Status.GetDescription() %></td>
				<td style="vertical-align:top">
					<a href="admin.plugins.manage-orders.view-order.aspx?selected=<%# Container.DataItem.Path %>">Details</a>
				</td>
			</tr>
		</ItemTemplate>
		<EmptyDataTemplate>
		    There are no search results to show for these values
		</EmptyDataTemplate>
	</zeus:TypedListView>
	
	  <asp:DataPager ID="dpgSearchResultsPager" runat="server" PageSize="30" PagedControlID="lsvOrders">
		<Fields>
      <asp:NumericPagerField ButtonCount="10" ButtonType="Link" />
    </Fields>
	</asp:DataPager>
</asp:Content>