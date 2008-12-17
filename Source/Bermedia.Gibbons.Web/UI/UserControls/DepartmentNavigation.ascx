<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DepartmentNavigation.ascx.cs" Inherits="Bermedia.Gibbons.Web.UI.UserControls.DepartmentNavigation" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Zeus" %>
<h2><%= this.Department.Title %></h2>

<asp:Label runat="server" AssociatedControlID="txtSearchText"><strong>Search</strong></asp:Label><br />
<asp:TextBox runat="server" ID="txtSearchText" MaxLength="50" CssClass="searchMe" /><br />
<asp:DropDownList runat="server" ID="ddlSearchDepartment" CssClass="searchMe">
	<asp:ListItem Value="">All Departments</asp:ListItem>
</asp:DropDownList><br />
<asp:Button runat="server" ID="btnSearch" Text="Go" CssClass="mySubmit" />
			
<isis:TypedListView runat="server" ID="rptChildPages" DataSource='<%# ((Zeus.ContentItem) this.Department).Children.OfType<Bermedia.Gibbons.Web.Items.Page>() %>' DataItemTypeName="Zeus.ContentItem">
	<LayoutTemplate>
		<ul>
			<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
		</ul>
	</LayoutTemplate>
	<ItemTemplate>
		<li><a href="<%# Container.DataItem.Url %>"><%# Container.DataItem.Title %></a></li>
	</ItemTemplate>
</isis:TypedListView>

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

	<li><a href="<%= this.Department.GiftsUnder10Url %>">under $10</a></li>
	<li><a href="<%= this.Department.GiftsUnder20Url %>">under $20</a></li>
	<li><a href="<%= this.Department.GiftsUnder50Url %>">under $50</a></li>
</ul>