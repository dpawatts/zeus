<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="ManageAddressBook.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.ManageAddressBook" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
	<link rel="stylesheet" href="/assets/css/myaccount.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Manage Address Book</h1>
	
	<h2>Shipping Addresses</h2>
	<isis:TypedListView runat="server" ID="lsvShippingAddresses" DataItemTypeName="Bermedia.Gibbons.Web.Items.Address, Bermedia.Gibbons.Web" DataKeyNames="ID" GroupItemCount="3" OnItemDeleting="lsvShippingAddresses_ItemDeleting">
		<LayoutTemplate>
			<table width="100%" border="0" cellpadding="2" cellspacing="0">
				<asp:PlaceHolder runat="server" ID="groupPlaceholder" />
			</table>
		</LayoutTemplate>
		<GroupTemplate>
			<tr>
				<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
			</tr>
		</GroupTemplate>
		<ItemTemplate>
			<td width="33%">
				<p>
					<%# Container.DataItem.AddressLine1 %><br />
					<%# Container.DataItem.AddressLine2 %><br />
					<%# Container.DataItem.City %><br />
					<%# Container.DataItem.ParishState %><br />
					<%# Container.DataItem.Zip %><br />
					<%# Container.DataItem.Country.Title %><br />
					<%# Container.DataItem.PhoneNumber %><br />
				</p>
				<a href="<%# new Zeus.Web.Url(this.CurrentItem.Url).AppendSegment("edit-address").AppendQuery("id", Container.DataItem.ID) %>">
					<sitdap:DynamicImage runat="server" ID="btnEditAddress" TemplateName="FixedWidthButton" AlternateText="edit this address">
						<Layers>
							<sitdap:TextLayer Name="Text" Text="edit this address" />
						</Layers>
					</sitdap:DynamicImage>
				</a>
				<br />
				<sitdap:DynamicImageButton runat="server" ID="btnDeleteAddress" CommandName="Delete" CommandArgument='<%# Container.DataItem.ID %>' TemplateName="FixedWidthButton" AlternateText="delete this address">
					<Layers>
						<sitdap:TextLayer Name="Text" Text="delete this address" />
					</Layers>
				</sitdap:DynamicImageButton>
			</td>
		</ItemTemplate>
		<EmptyDataTemplate>
			<p><b>There are no shipping addresses currently in your address book.</b></p>
		</EmptyDataTemplate>
	</isis:TypedListView>
	
	<br /><br />
	<h2>Billing Addresses</h2>
	<isis:TypedListView runat="server" ID="lsvBillingAddresses" DataItemTypeName="Bermedia.Gibbons.Web.Items.Address, Bermedia.Gibbons.Web" GroupItemCount="3" OnItemDeleting="lsvBillingAddresses_ItemDeleting">
		<LayoutTemplate>
			<table width="100%" border="0" cellpadding="2" cellspacing="0">
				<asp:PlaceHolder runat="server" ID="groupPlaceholder" />
			</table>
		</LayoutTemplate>
		<GroupTemplate>
			<tr>
				<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
			</tr>
		</GroupTemplate>
		<ItemTemplate>
			<td width="33%">
				<p>
					<%# Container.DataItem.AddressLine1 %><br />
					<%# Container.DataItem.AddressLine2 %><br />
					<%# Container.DataItem.City %><br />
					<%# Container.DataItem.ParishState %><br />
					<%# Container.DataItem.Zip %><br />
					<%# Container.DataItem.Country.Title %><br />
					<%# Container.DataItem.PhoneNumber %><br />
				</p>
				<a href="<%# new Zeus.Web.Url(this.CurrentItem.Url).AppendSegment("edit-address").AppendQuery("id", Container.DataItem.ID) %>">
					<sitdap:DynamicImage runat="server" ID="btnEditAddress" TemplateName="FixedWidthButton" AlternateText="edit this address">
						<Layers>
							<sitdap:TextLayer Name="Text" Text="edit this address" />
						</Layers>
					</sitdap:DynamicImage>
				</a>
				<br />
				<sitdap:DynamicImageButton runat="server" ID="btnDeleteAddress" CommandName="Delete" CommandArgument='<%# Container.DataItem.ID %>' TemplateName="FixedWidthButton" AlternateText="delete this address">
					<Layers>
						<sitdap:TextLayer Name="Text" Text="delete this address" />
					</Layers>
				</sitdap:DynamicImageButton>
			</td>
		</ItemTemplate>
		<EmptyDataTemplate>
			<p><b>There are no billing addresses currently in your address book.</b></p>
		</EmptyDataTemplate>
	</isis:TypedListView>
</asp:Content>
