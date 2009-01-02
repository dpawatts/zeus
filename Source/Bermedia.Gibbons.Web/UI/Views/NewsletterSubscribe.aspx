<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="NewsletterSubscribe.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.NewsletterSubscribe" %>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Email Exclusives</h1>
	
	<asp:PlaceHolder runat="server" ID="plcForm">		
		<p>Please enter your e-mail address below to receive exclusive offers by e-mail from Gibbons.bm:</p>

		<table border="0" cellpadding="1" cellspacing="0">
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtEmailAddress" Text="Your e-mail address" /></td>
				<td>
					<asp:TextBox runat="server" ID="txtEmailAddress" MaxLength="500" />
					<asp:RequiredFieldValidator runat="server" ID="rfvEmailAddress" ControlToValidate="txtEmailAddress" Text="*" ErrorMessage="E-mail address is required" />
					<asp:RegularExpressionValidator runat="server" ID="revEmailAddress" ControlToValidate="txtEmailAddress" ValidationExpression="\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}\b" Text="*" ErrorMessage="This is not a valid e-mail address" />
					<asp:CustomValidator runat="server" ID="csvEmailAddress" OnServerValidate="csvEmailAddress_ServerValidate" Text="*" ErrorMessage="This e-mail address is already in use" />
				</td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtFirstName" Text="Your first name" /></td>
				<td>
					<asp:TextBox runat="server" ID="txtFirstName" />
					<asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName" Text="*" ErrorMessage="First name is required" />
				</td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtLastName" Text="Your last name" /></td>
				<td>
					<asp:TextBox runat="server" ID="txtLastName" />
					<asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName" Text="*" ErrorMessage="Last name is required" />
				</td>
			</tr>
			<tr>
				<td></td>
				<td>
					<p><asp:ValidationSummary runat="server" ID="vlsSummary" /></p>
					<asp:Button runat="server" ID="btnSubscribe" Text="Subscribe" OnClick="btnSubscribe_Click" CssClass="cButton" />
				</td>
			</tr>
		</table>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder runat="server" ID="plcConfirmation" Visible="false">
		<p>Thank you. You should receive a confirmation e-mail shortly.</p>
	</asp:PlaceHolder>
</asp:Content>
