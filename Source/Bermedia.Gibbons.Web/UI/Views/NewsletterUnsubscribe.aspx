<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="NewsletterUnsubscribe.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.NewsletterUnsubscribe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1>Unsubscribe From Email Exclusives</h1>
	
	<asp:PlaceHolder runat="server" ID="plcForm">
		<p>Please enter your e-mail address below to remove your newsletter subscription:</p>

		<table border="0" cellpadding="1" cellspacing="0">		
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtEmailAddress" Text="Your e-mail address" /></td>
				<td>
					<asp:TextBox runat="server" ID="txtEmailAddress" MaxLength="500" />
					<asp:RequiredFieldValidator runat="server" ID="rfvEmailAddress" ControlToValidate="txtEmailAddress" Text="*" ErrorMessage="E-mail address is required" />
					<asp:CustomValidator runat="server" ID="csvEmailAddress" OnServerValidate="csvEmailAddress_ServerValidate" Text="*" ErrorMessage="This e-mail address is not currently receiving newsletters" />
				</td>
			</tr>
			<tr>
				<td></td>
				<td>
					<p><asp:ValidationSummary runat="server" ID="vlsSummary" /></p>
					<asp:Button runat="server" ID="btnUnsubscribe" Text="Unsubscribe" OnClick="btnUnsubscribe_Click" CssClass="cButton" />
				</td>
			</tr>
		</table>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder runat="server" ID="plcConfirmation" Visible="false">
		<p>Thank you. You have been unsubscribed from our email exclusives.</p>
	</asp:PlaceHolder>
</asp:Content>