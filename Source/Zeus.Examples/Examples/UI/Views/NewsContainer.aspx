<%@ Page Language="C#" MasterPageFile="~/Examples/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="NewsContainer.aspx.cs" Inherits="Zeus.Examples.UI.Views.NewsContainer" %>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<asp:ListView runat="server" DataSourceID="cdsNews">
		<LayoutTemplate>
			<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
		</LayoutTemplate>
		<ItemTemplate>
			<div>
				<span class="date"><%# Eval("Published") %></span>
				<a href="<%# Eval("Url") %>"><%# Eval("Title") %></a>
				<p><%# Eval("Introduction") %></p>
			</div>
		</ItemTemplate>
	</asp:ListView>
	<zeus:ContentDataSource id="cdsNews" runat="server" OfType="Zeus.Examples.Items.NewsItem" />
</asp:Content>