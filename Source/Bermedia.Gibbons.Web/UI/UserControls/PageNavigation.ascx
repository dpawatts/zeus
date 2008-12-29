<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageNavigation.ascx.cs" Inherits="Bermedia.Gibbons.Web.UI.UserControls.PageNavigation" %>
<%@ Register TagPrefix="gibbons" TagName="Search" Src="Search.ascx" %>

<gibbons:Search runat="server" />

<isis:TypedListView runat="server" ID="lsvSubNavigation" DataItemTypeName="Zeus.ContentItem">
	<LayoutTemplate>
		<ul>
			<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
		</ul>
	</LayoutTemplate>
	<ItemTemplate>
		<li><a href="<%# Container.DataItem.Url %>"><strong><%# Container.DataItem.Title %></strong></a></li>
	</ItemTemplate>
</isis:TypedListView>