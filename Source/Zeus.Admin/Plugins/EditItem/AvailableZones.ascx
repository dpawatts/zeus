<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AvailableZones.ascx.cs" Inherits="Zeus.Admin.Plugins.EditItem.AvailableZones" %>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Web.UI.HtmlControls" Assembly="Zeus" %>
<zeus:HtmlFieldSet runat="server" Legend="Zones" class="zonesBox">
	<asp:ListView ID="rptZones" runat="server">
		<LayoutTemplate>
			<dl>
				<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
			</dl>
		</LayoutTemplate>
		<ItemTemplate>
			<dt>
				<asp:HyperLink ID="hlNew" runat="server" ToolTip="New item" NavigateUrl="<%# GetNewDataItemUrl(Container.DataItem) %>" Text="<%# GetNewDataItemText(Container.DataItem) %>" />
				<strong><%# GetZoneString((string)Eval("ZoneName")) ?? Eval("Title") %></strong>
			</dt>
			<asp:ListView ID="rptItems" runat="server" DataSource="<%# GetItemsInZone(Container.DataItem) %>">
				<LayoutTemplate>
					<dd class="items">
						<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
					</dd>
				</LayoutTemplate>
				<ItemTemplate>
					<div class="edit">
						<asp:HyperLink runat="server" Text="<%# GetEditDataItemText(Container.DataItem) %>" NavigateUrl="<%# GetEditDataItemUrl(Container.DataItem) %>" />
					</div>
				</ItemTemplate>
			</asp:ListView>
		</ItemTemplate>
	</asp:ListView>
</zeus:HtmlFieldSet>