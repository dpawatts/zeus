<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSubNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1>Sign In or create a Gibbons Online account</h1>
	
	<h2>Returning Guests</h2>
	
	<p>If you have an account with Gibbons Online, please sign in:</p>
	
	<asp:Login runat="server" ID="lgnLogin" OnLoggedIn="lgnLogin_LoggedIn">
		<LayoutTemplate>
			<asp:Panel runat="server" DefaultButton="btnLogin">
				<table cellspacing="0" cellpadding="1" border="0">
					<tr>
						<td width="130"><asp:Label runat="server" AssociatedControlID="UserName">Your e-mail address</asp:Label></td>
						<td><asp:TextBox runat="server" ID="UserName" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" Text="*" /></td>
					</tr>
					<tr>
						<td><asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label></td>
						<td><asp:TextBox runat="server" ID="Password" TextMode="Password" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" Text="*" /></td>
					</tr>
					<tr>
						<td></td>
						<td><asp:Label runat="server" ID="FailureText" CssClass="warning" /></td>
					</tr>
					<tr>
						<td></td>
						<td>
							<sitdap:DynamicImageButton runat="server" ID="btnLogin" CommandName="Login" TemplateName="Button" AlternateText="Sign In">
								<Layers>
									<sitdap:TextLayer Name="Text" Text="sign in" />
								</Layers>
							</sitdap:DynamicImageButton>
						</td>
					</tr>
				</table>
			</asp:Panel>
		</LayoutTemplate>
	</asp:Login>
	
	<p><a href="forgotten-password.aspx">Forgot your password?</a></p>
			
	<p>&nbsp;</p>
	<h2>New To Gibbons Online?</h2>
	<p>If you do not have an account, please create one now.</p>

	<p><a href="register.aspx?ReturnUrl=<%= Server.UrlEncode(Request.QueryString["ReturnUrl"]) %>">
		<sitdap:DynamicImage runat="server" TemplateName="Button" AlternateText="Create an Account">
			<Layers>
				<sitdap:TextLayer Name="Text" Text="create an account" />
			</Layers>
		</sitdap:DynamicImage>
	</a></p>
</asp:Content>
