<%@ Page Language="C#" MasterPageFile="~/Admin/PreviewFrame.Master" AutoEventWireup="true" CodeBehind="SalesByCategory.aspx.cs" Inherits="Bermedia.Gibbons.Web.Plugins.Reports.SalesByCategory" %>
<%@ Register TagPrefix="gibbons" TagName="DateRange" Src="~/Plugins/Reports/DateRange.ascx" %>
<asp:Content ContentPlaceHolderID="Head" runat="server">
	<link rel="stylesheet" href="/admin/assets/css/shared.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	<link rel="stylesheet" href="/admin/assets/css/view.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	
	<script type="text/javascript" src="/admin/assets/js/jquery.js"></script>
	<script type="text/javascript" src="/admin/assets/js/zeus.js"></script>
	<script type="text/javascript" src="/admin/assets/js/view.js"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<gibbons:DateRange runat="server" ID="uscDateRange" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<h2>Sales by Category</h2>
	
	<isis:TypedListView runat="server" ID="lsvSales" DataItemTypeName="Bermedia.Gibbons.Web.Plugins.Reports.SalesByCategory+SalesData, Bermedia.Gibbons.Web">
		<LayoutTemplate>
			<table class="tb">
				<tr class="titles">
					<th>Category of Merchandise</th>
					<th>Units Sold</th>
				</tr>
				<tr runat="server" ID="itemPlaceholder" />
			</table>
		</LayoutTemplate>
		<ItemTemplate>
			<tr>
				<td><%# Container.DataItem.Category %></td>
				<td style="text-align:right"><%# Container.DataItem.UnitsSold %></td>
			</tr>
		</ItemTemplate>
	</isis:TypedListView>
</asp:Content>