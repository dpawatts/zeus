<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="EditAddress.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.EditAddress" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
	<link rel="stylesheet" href="/assets/css/myaccount.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Edit Address</h1>
	
	<asp:Panel runat="server" DefaultButton="btnSave">
		<table border="0" cellpadding="1" cellspacing="0">
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtAddress1">Address 1</asp:Label></td>
				<td><asp:TextBox runat="server" ID="txtAddress1" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress1" ErrorMessage="Address 1 is required" Text="*" /></td>
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
				<td><asp:TextBox runat="server" ID="txtZip" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="txtZip" ErrorMessage="ZIP is required" Text="*" /></td>
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
				<td><asp:TextBox runat="server" ID="txtPhone" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhone" ErrorMessage="Phone is required" Text="*" /></td>
			</tr>
			<tr>
				<td></td>
				<td>
					<p><asp:ValidationSummary runat="server" ID="vlsSummary" /></p>
					<br />
					
					<sitdap:DynamicImageButton runat="server" ID="btnSave" OnClick="btnSave_Click" TemplateName="Button" AlternateText="save">
						<Layers>
							<sitdap:TextLayer Name="Text" Text="save" />
						</Layers>
					</sitdap:DynamicImageButton>
				</td>
			</tr>
		</table>
	</asp:Panel>
</asp:Content>
