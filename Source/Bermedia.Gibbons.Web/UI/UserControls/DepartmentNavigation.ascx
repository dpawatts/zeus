<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DepartmentNavigation.ascx.cs" Inherits="Bermedia.Gibbons.Web.UI.UserControls.DepartmentNavigation" %>
<%@ Register TagPrefix="gibbons" TagName="Search" Src="Search.ascx" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Zeus" %>
<h2><%= this.Department.Title %></h2>

<gibbons:Search runat="server" />

<ul>
	<li><a href="<%= this.Department.Url %>"><%= this.Department.Title %> Homepage</a></li>
	<isis:TypedRepeater runat="server" ID="rptChildPages" DataSource='<%# ((Zeus.ContentItem) this.Department).Children.OfType<Bermedia.Gibbons.Web.Items.Page>().Where(p => p.Visible) %>' DataItemTypeName="Zeus.ContentItem">
		<ItemTemplate>
			<li><a href="<%# Container.DataItem.Url %>"><%# Container.DataItem.Title %></a></li>
		</ItemTemplate>
	</isis:TypedRepeater>
</ul>

<isis:TypedListView runat="server" ID="rptCategories" DataSource='<%# ((Zeus.ContentItem) this.Department).GetChildren().OfType<Bermedia.Gibbons.Web.Items.BaseCategory>() %>' DataItemTypeName="Zeus.ContentItem">
	<LayoutTemplate>
		<ul>
			<li>Shop by category</li>
			<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
		</ul>
	</LayoutTemplate>
	<ItemTemplate>
		<li><a href="<%# Container.DataItem.Url %>"><%# Container.DataItem.Title %></a> •</li>
	</ItemTemplate>
</isis:TypedListView>
	
<ul>
	<li>Looking for a gift?</li>

	<li><a href="<%= this.Department.GiftsUnder25Url %>">under $25</a></li>
	<li><a href="<%= this.Department.GiftsUnder50Url %>">under $50</a></li>
	<li><a href="<%= this.Department.GiftsUnder100Url %>">under $100</a></li>
</ul>