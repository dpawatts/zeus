<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CheckoutBillingAddress.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.CheckoutBillingAddress" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1>Billing Address</h1>
	<p>Please select a billing address from your address book (below) or enter a new billing address.</p>
	
	<asp:Panel runat="server" DefaultButton="btnNext">
		<table border="0" cellpadding="1" cellspacing="0">
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtAddress1">Address 1</asp:Label></td>
				<td><asp:TextBox runat="server" ID="txtAddress1" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress1" Text="*" ValidationGroup="cuwRegister" /></td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtAddress2">Address 2</asp:Label></td>
				<td><asp:TextBox runat="server" ID="txtAddress2" /></td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtCity">City</asp:Label></td>
				<td><asp:TextBox runat="server" ID="txtCity" /></td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtParishState">Parish or State</asp:Label></td>
				<td><asp:TextBox runat="server" ID="txtParishState" /></td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtZip">ZIP</asp:Label></td>
				<td><asp:TextBox runat="server" ID="txtZip" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="txtZip" Text="*" ValidationGroup="cuwRegister" /></td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="ddlCountry">Country</asp:Label></td>
				<td>
					<asp:DropDownList runat="server" ID="ddlCountry" DataSourceID="cdsCountries" DataValueField="ID" DataTextField="Title" AppendDataBoundItems="true" />
					<zeus:ContentDataSource runat="server" ID="cdsCountries" OfType="Bermedia.Gibbons.Web.Items.Country" Axis="Descendant" Query="RootItem" />
				</td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtPhone">Phone</asp:Label></td>
				<td><asp:TextBox runat="server" ID="txtPhone" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhone" Text="*" ValidationGroup="cuwRegister" /></td>
			</tr>
			<tr>
				<td></td>
				<td>
					<sitdap:DynamicImageButton runat="server" ID="btnNext" OnClick="btnNext_Click" TemplateName="Button" AlternateText="next" ValidationGroup="cuwRegister">
						<Layers>
							<sitdap:TextLayer Name="Text" Text="next" />
						</Layers>
					</sitdap:DynamicImageButton>
				</td>
			</tr>
		</table>
	</asp:Panel>
	
	<br />
	
	<h2>Address Book</h2>
	<isis:TypedListView runat="server" ID="lsvAddressBook" DataSource='<%$ Code:this.Customer.BillingAddresses %>' DataItemTypeName="Bermedia.Gibbons.Web.Items.Address, Bermedia.Gibbons.Web">
		<LayoutTemplate>
			<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
		</LayoutTemplate>
		<ItemTemplate>
			<p>
				<%# Container.DataItem.AddressLine1 %><br />
				<%# Container.DataItem.AddressLine2 %><br />
				<%# Container.DataItem.City %><br />
				<%# Container.DataItem.ParishState %><br />
				<%# Container.DataItem.Zip %><br />
				<%# Container.DataItem.Country.Title %><br />
				<%# Container.DataItem.PhoneNumber %><br />
			</p>
			<sitdap:DynamicImageButton runat="server" ID="btnUseAddress" CommandArgument='<%# Container.DataItem.ID %>' TemplateName="Button" AlternateText="use address" OnClick="btnUseAddress_Click" CausesValidation="false">
				<Layers>
					<sitdap:TextLayer Name="Text" Text="use address" />
				</Layers>
			</sitdap:DynamicImageButton>
		</ItemTemplate>
		<EmptyDataTemplate>
			<p><b>There are no billing addresses currently in your address book.</b></p>
		</EmptyDataTemplate>
	</isis:TypedListView>
</asp:Content>
