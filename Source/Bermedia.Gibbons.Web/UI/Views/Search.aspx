<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.Search" %>
<%@ Register TagPrefix="gibbons" TagName="PageNavigation" Src="../UserControls/PageNavigation.ascx" %>
<%@ Register TagPrefix="gibbons" TagName="ProductListing" Src="../UserControls/ProductListing.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:PageNavigation runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1>Search Results for "<%= Request.QueryString["q"] %>"</h1>
	<asp:Repeater runat="server" ID="rptSearchResults">
		<ItemTemplate>
			<p><a href="<%# new Zeus.Web.Url((string) Eval("Key.Url")).AppendSegment("search").AppendQuery("q=" + Request.QueryString["q"]).ToString() %>"><strong><%# Eval("Key.Title") %></strong></a></p>
			<ul>
				<asp:Repeater runat="server" DataSource='<%# Container.DataItem %>'>
					<ItemTemplate>
						<li><a href="<%# Eval("Url") %>"><%# Eval("Title") %></a></li>
					</ItemTemplate>
				</asp:Repeater>
			</ul>
		</ItemTemplate>
	</asp:Repeater>
</asp:Content>
