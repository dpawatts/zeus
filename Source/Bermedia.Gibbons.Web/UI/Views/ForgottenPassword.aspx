<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="ForgottenPassword.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.ForgottenPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSubNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1>Forgot Your Password?</h1>
	
	<asp:PasswordRecovery runat="server" ID="pwrPasswordRecovery" MailDefinition-Subject="Gibbons Company Account" MailDefinition-BodyFileName="PasswordRecoveryMessage.txt"
		UserNameFailureText="We are very sorry but the email you have entered does not match any in our system. Please try again with a different email address or check the spelling of the email address you entered.">
		<UserNameTemplate>
			<p>If you have forgotten you password, please enter the email address you entered when you signed
			up with our website and your password will be sent to you.</p>
			
			<table cellspacing="0" cellpadding="1" border="0">
				<tr>
					<td width="130"><asp:Label runat="server" AssociatedControlID="UserName">Your e-mail address</asp:Label></td>
					<td><asp:TextBox runat="server" ID="UserName" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" Text="*" ErrorMessage="E-mail address is required" /></td>
				</tr>
				<tr>
					<td></td>
					<td><asp:Label runat="server" ID="FailureText" CssClass="error" /></td>
				</tr>
				<tr>
					<td></td>
					<td>
						<p><asp:ValidationSummary runat="server" ID="vlsSummary" /></p>
						
						<sitdap:DynamicImageButton runat="server" CommandName="Submit" TemplateName="Button" AlternateText="submit">
							<Layers>
								<sitdap:TextLayer Name="Text" Text="submit" />
							</Layers>
						</sitdap:DynamicImageButton>
					</td>
				</tr>
			</table>
		</UserNameTemplate>
		<QuestionTemplate>
			<p>Answer the following question to receive your password.</p>
			
			<table cellspacing="0" cellpadding="1" border="0">
				<tr>
					<td width="130">Question</td>
					<td><asp:Literal runat="server" ID="Question" /></td>
				</tr>
				<tr>
					<td>Answer</td>
					<td>
						<asp:TextBox runat="server" ID="Answer" />
						<asp:RequiredFieldValidator runat="server" ControlToValidate="Answer" Text="*" ErrorMessage="Answer is required" />
					</td>
				</tr>
				<tr>
					<td></td>
					<td><asp:Label runat="server" ID="FailureText" CssClass="error" /></td>
				</tr>
				<tr>
					<td></td>
					<td>
						<p><asp:ValidationSummary runat="server" ID="vlsSummary" /></p>
						
						<sitdap:DynamicImageButton runat="server" CommandName="Submit" TemplateName="Button" AlternateText="send my password">
							<Layers>
								<sitdap:TextLayer Name="Text" Text="send my password" />
							</Layers>
						</sitdap:DynamicImageButton>
					</td>
				</tr>
			</table>
		</QuestionTemplate>
	</asp:PasswordRecovery>
</asp:Content>
