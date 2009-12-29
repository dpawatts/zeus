<%@ Page Title="Add New Item" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.NewItem.Default" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.WebControls" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register TagPrefix="ext" Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" %>
<%@ Import Namespace="Zeus.ContentTypes" %>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" ImageResourceName="Zeus.Admin.Assets.Images.Icons.cross.png" CssClass="negative" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<ext:ScriptManager runat="server" Theme="Gray" />
	
	<ext:TabPanel runat="server" ID="tbcTabs" BodyStyle="padding:5px">
		<Tabs>
			<ext:Tab runat="server" ID="tbpType" Title="Type">
				<Body>
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
				</Body>
			</ext:Tab>
			
			<ext:Tab runat="server" ID="tbpPosition" Title="Position">
				<Body>
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
				</Body>
			</ext:Tab>
			
			<ext:Tab runat="server" ID="tbpZone" Title="Zone">
				<Body>
					<asp:Label ID="lblZone" runat="server" Text="Create new item in zone" />
					<asp:RadioButtonList runat="server" ID="rblZone" CssClass="zones" DataTextField="Title" DataValueField="ZoneName" AutoPostBack="true" OnSelectedIndexChanged="rblZone_OnSelectedIndexChanged">
						<asp:ListItem Value="" Selected="true">Default</asp:ListItem>
					</asp:RadioButtonList>
				</Body>
			</ext:Tab>
		</Tabs>
	</ext:TabPanel>
</asp:Content>