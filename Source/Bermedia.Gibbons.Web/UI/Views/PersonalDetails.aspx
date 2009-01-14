<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="PersonalDetails.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.PersonalDetails" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
	<link rel="stylesheet" href="/assets/css/myaccount.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Personal Details</h1>

	<p>If you want to change the name, e-mail address, or password associated with your Gibbons.bm customer account, 
	you may do so below. Be sure to click the <strong>Save Changes</strong> button when you are done.
	<strong>You will need to login again after making any changes.</strong></p>

	<hr color="#ddddcc" />

	<table width="90%" border="0" align="center" cellpadding="4" cellspacing="2">
		<tr>
			<td align="right"><strong>New first name:</strong></td>
			<td><asp:TextBox runat="server" ID="txtNewFirstName" /> <asp:RequiredFieldValidator runat="server" ID="rfvNewFirstName" ControlToValidate="txtNewFirstName" Text="*" ErrorMessage="First name is required" /></td>
		</tr>
		<tr>
			<td align="right"><strong>New surname:</strong></td>
			<td><asp:TextBox runat="server" ID="txtNewLastName" /> <asp:RequiredFieldValidator runat="server" ID="rfvNewLastName" ControlToValidate="txtNewLastName" Text="*" ErrorMessage="Surname is required" /></td>
		</tr>
		<tr>
			<td align="right"><strong>New e-mail address:</strong></td>
			<td>
				<asp:TextBox runat="server" ID="txtNewEmail" />
				 <asp:RequiredFieldValidator runat="server" ID="rfvNewEmail" ControlToValidate="txtNewEmail" Text="*" ErrorMessage="E-mail address is required" />
				<asp:CustomValidator runat="server" ID="csvNewEmail" OnServerValidate="csvNewEmail_ServerValidate" ControlToValidate="txtNewEmail" ErrorMessage="This e-mail address is already in use" Text="*" />
			</td>
		</tr>
		<tr> 
			<td height="18">&nbsp;</td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td align="right"><strong>Old password:</strong></td>
			<td>
				<asp:TextBox runat="server" ID="txtOldPassword" TextMode="Password" />
				<asp:CustomValidator runat="server" ID="csvOldPassword" OnServerValidate="csvOldPassword_ServerValidate" ValidateEmptyText="true" ControlToValidate="txtOldPassword" ErrorMessage="Please enter your old password" Text="*" />
			</td>
		</tr>
		<tr> 
			<td height="18">&nbsp;</td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td align="right"><strong>New password:</strong></td>
			<td><asp:TextBox runat="server" ID="txtNewPassword" TextMode="Password" /></td>
		</tr>
		<tr>
			<td align="right"><strong>Re-enter password:</strong></td>
			<td>
				<asp:TextBox runat="server" ID="txtNewPasswordConfirm" TextMode="Password" />
				<asp:CompareValidator runat="server" Text="*" ErrorMessage="The password fields must match."
					ControlToCompare="txtNewPasswordConfirm" ControlToValidate="txtNewPassword" />
			</td>
		</tr>
		<tr>
			<td></td>
			<td>
				<p><asp:ValidationSummary runat="server" ID="vlsSummary" /></p>
				<sitdap:DynamicImageButton runat="server" ID="btnSaveChanges" OnClick="btnSaveChanges_Click" TemplateName="Button" AlternateText="save changes">
					<Layers>
						<sitdap:TextLayer Name="Text" Text="save changes" />
					</Layers>
				</sitdap:DynamicImageButton>
			</td>
		</tr>
	</table>
</asp:Content>