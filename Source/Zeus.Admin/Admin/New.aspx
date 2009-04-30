<%@ Page Title="Add New Item" Language="C#" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="Zeus.Admin.New" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Import Namespace="Zeus.ContentTypes" %>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" ImageResourceName="Zeus.Admin.Assets.Images.Icons.cross.png" CssClass="negative" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<zeus:TabControl runat="server" ID="tbcTabs">
		<zeus:TabItem runat="server" ID="tbpType" ToolTip="Type">
			<asp:ListView runat="server" ID="lsvChildTypes">
				<LayoutTemplate>
					<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
				</LayoutTemplate>
				<ItemTemplate>
					<p>
						<a href="<%# GetEditUrl((ContentType) Container.DataItem) %>"><img runat="server" src='<%# Eval("IconUrl") %>' alt="" /></a>
						<strong><a href="<%# GetEditUrl((ContentType) Container.DataItem) %>"><%# Eval("Title")%></a></strong>
						<%# Eval("ContentTypeAttribute.Description") %>
					</p>
				</ItemTemplate>
				<EmptyDataTemplate>
					<p>You cannot add an item below this location.</p>
				</EmptyDataTemplate>
			</asp:ListView>
		</zeus:TabItem>
		
		<zeus:TabItem runat="server" ID="tbpPosition" ToolTip="Position">
			<asp:Label ID="lblPosition" runat="server" Text="Select at what position to create the item" />
			<asp:RadioButtonList runat="server" ID="rblPosition" AutoPostBack="true" OnSelectedIndexChanged="rblPosition_OnSelectedIndexChanged" CssClass="position">
				<asp:ListItem Value="0">
					Before the selected item (at the same depth)
					<blockquote>
						<ul>
							<li>other items</li>
							<li><em>new item</em></li>
							<li>
								<strong>selected item</strong>
								<ul>
									<li>other item</li>
								</ul>
							</li>
						</ul>
					</blockquote>
				</asp:ListItem>
				<asp:ListItem Value="1" Selected="true">
					Below the selected item (one level deeper)
					<blockquote>
						<ul>
							<li>
								<strong>selected item</strong>
								<ul>
									<li>other item</li>
									<li><em>new item</em></li>
								</ul>
							</li>
							<li>other item</li>
						</ul>
					</blockquote>
				</asp:ListItem>
				<asp:ListItem Value="2">
					After the selected item (at the same depth)
					<blockquote>
						<ul>
							<li>
								<strong>selected item</strong>
								<ul>
									<li>other item</li>
								</ul>
							</li>
							<li><em>new item</em></li>
							<li>other items</li>
						</ul>
					</blockquote>
				</asp:ListItem>
			</asp:RadioButtonList>
		</zeus:TabItem>
		
		<zeus:TabItem runat="server" ID="tbpZone" ToolTip="Zone">
			<asp:Label ID="lblZone" runat="server" Text="Create new item in zone" />
			<asp:RadioButtonList runat="server" ID="rblZone" CssClass="zones" DataTextField="Title" DataValueField="ZoneName" AutoPostBack="true" OnSelectedIndexChanged="rblZone_OnSelectedIndexChanged">
				<asp:ListItem Value="" Selected="true">Default</asp:ListItem>
			</asp:RadioButtonList>
		</zeus:TabItem>
	</zeus:TabControl>
</asp:Content>