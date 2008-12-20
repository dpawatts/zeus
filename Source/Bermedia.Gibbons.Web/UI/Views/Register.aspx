<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSubNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1>Create a Gibbons Online account</h1>
	
	<asp:Panel runat="server" DefaultButton="cuwRegister$__CustomNav0$btnCreateAccount">
		<table cellspacing="0" cellpadding="1" border="0">
			<asp:CreateUserWizard runat="server" ID="cuwRegister" RequireEmail="false" OnCreatedUser="cuwRegister_CreatedUser"
				OnContinueButtonClick="cuwRegister_ContinueButtonClick" LoginCreatedUser="false" DuplicateUserNameErrorMessage="This e-mail address is already in use">
				<WizardSteps>
					<asp:CreateUserWizardStep ID="cusStep1">
						<ContentTemplate>
							<tr>
								<td width="180"><asp:Label runat="server" AssociatedControlID="txtFirstName">First name</asp:Label></td>
								<td><asp:TextBox runat="server" ID="txtFirstName" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" Text="*" ValidationGroup="cuwRegister" /></td>
							</tr>
							<tr>
								<td><asp:Label runat="server" AssociatedControlID="txtLastName">Surname</asp:Label></td>
								<td><asp:TextBox runat="server" ID="txtLastName" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" Text="*" ValidationGroup="cuwRegister" /></td>
							</tr>
							<tr>
								<td><asp:Label runat="server" AssociatedControlID="UserName">E-mail address</asp:Label></td>
								<td><asp:TextBox runat="server" ID="UserName" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" Text="*" ValidationGroup="cuwRegister" /></td>
							</tr>
							<tr>
								<td><asp:Label runat="server" AssociatedControlID="ConfirmUserName">Re-enter your e-mail address</asp:Label></td>
								<td><asp:TextBox runat="server" ID="ConfirmUserName" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmUserName" Text="*" ValidationGroup="cuwRegister" /></td>
							</tr>
							<tr>
								<td></td>
								<td>
									<asp:CompareValidator runat="server" ErrorMessage="Both e-mail address fields must match."
										ControlToCompare="ConfirmUserName" ControlToValidate="UserName" ValidationGroup="cuwRegister" />
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:CheckBox runat="server" ID="chkReceiveOffers" TextAlign="Left" Text="Would you like to sign-up for Gibbons exclusive e-mail? Be the first to know about events, web only offers, the latest trends and news, as well as store events. Sign up here:" />
									<a href="#">Privacy Policy</a>
								</td>
							</tr>
							<tr>
								<td><asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label></td>
								<td><asp:TextBox runat="server" ID="Password" TextMode="Password" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" Text="*" ValidationGroup="cuwRegister" /></td>
							</tr>
							<tr>
								<td><asp:Label runat="server" AssociatedControlID="ConfirmPassword">Confirm Password</asp:Label></td>
								<td><asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword" Text="*" ValidationGroup="cuwRegister" /></td>
							</tr>
							<tr>
								<td></td>
								<td>
									<asp:CompareValidator runat="server" ErrorMessage="The Password and Confirm Password fields must match."
										ControlToCompare="ConfirmPassword" ControlToValidate="Password" ValidationGroup="cuwRegister" />
								</td>
							</tr>
							<tr>
								<td><asp:Label runat="server" AssociatedControlID="Question">Choose a password hint</asp:Label></td>
								<td>
									<asp:DropDownList runat="server" ID="Question">
										<asp:ListItem Value="">Please choose a question...</asp:ListItem>
										<asp:ListItem>Which city were you born in?</asp:ListItem>
										<asp:ListItem>What is the first school you attended?</asp:ListItem>
										<asp:ListItem>What is your mother's maiden name?</asp:ListItem>
										<asp:ListItem>What is your best friend's pet's name?</asp:ListItem>
										<asp:ListItem>What is your favourite drink?</asp:ListItem>
										<asp:ListItem>What is your favourity cartoon character?</asp:ListItem>
										<asp:ListItem>What is your favourite colour?</asp:ListItem>
										<asp:ListItem>What is your favourite board game?</asp:ListItem>
										<asp:ListItem>What is your favourite flower?</asp:ListItem>
										<asp:ListItem>What is your favourite animal?</asp:ListItem>
										<asp:ListItem>Where was your favourite holiday?</asp:ListItem>
										<asp:ListItem>What is your favourite movie?</asp:ListItem>
										<asp:ListItem>What is your favourite restaurant?</asp:ListItem>
										<asp:ListItem>What is your favourite sport?</asp:ListItem>
										<asp:ListItem>What is your favourite TV show?</asp:ListItem>
										<asp:ListItem>Where is your favourite holiday destination?</asp:ListItem>
										<asp:ListItem>Who is your favourite author?</asp:ListItem>
										<asp:ListItem>What is the last name of your favourite athlete?</asp:ListItem>
										<asp:ListItem>Who is your favourite musical group or artist?</asp:ListItem>
										<asp:ListItem>What is your favourite flavour of ice creme?</asp:ListItem>
									</asp:DropDownList>
									<asp:RequiredFieldValidator runat="server" ControlToValidate="Question" Text="*" ValidationGroup="cuwRegister" />
								</td>
							</tr>
							<tr>
								<td><asp:Label runat="server" AssociatedControlID="Answer">Your password hint answer</asp:Label></td>
								<td><asp:TextBox runat="server" ID="Answer" /> <asp:RequiredFieldValidator runat="server" ControlToValidate="Answer" Text="*" ValidationGroup="cuwRegister" /></td>
							</tr>
							<tr>
								<td></td>
								<td><asp:Label runat="server" ID="ErrorMessage" CssClass="error" /></td>
							</tr>
							
							<tr>
								<td colspan="2">
									<iframe src="/tsandcs.htm" width="500px" height="200x" scrolling="auto"></iframe>
					
									<p><asp:CheckBox runat="server" ID="chkAgree" Text="I agree with the terms and conditions" /></p>
									<asp:CustomValidator runat="server" ID="csvTerms" OnServerValidate="csvTerms_ServerValidate" ErrorMessage="Please check the box to indicate you agree with the terms and conditions" ValidationGroup="Terms" />
								</td>
							</tr>
						</ContentTemplate>
						<CustomNavigationTemplate>
							<tr>
								<td></td>
								<td>
									<sitdap:DynamicImageButton runat="server" ID="btnCreateAccount" CommandName="MoveNext" TemplateName="Button" AlternateText="create account" ValidationGroup="cuwRegister">
										<Layers>
											<sitdap:TextLayer Name="Text" Text="create account" />
										</Layers>
									</sitdap:DynamicImageButton>
								</td>
							</tr>
						</CustomNavigationTemplate>
					</asp:CreateUserWizardStep>
					<asp:CompleteWizardStep runat="server" ID="cwsFinish" Title="">
						<CustomNavigationTemplate>
							<br />
							<sitdap:DynamicImageButton runat="server" CommandName="Continue" TemplateName="Button" AlternateText="continue">
								<Layers>
									<sitdap:TextLayer Name="Text" Text="continue" />
								</Layers>
							</sitdap:DynamicImageButton>
						</CustomNavigationTemplate>
					</asp:CompleteWizardStep>
				</WizardSteps>
			</asp:CreateUserWizard>
		</table>
	</asp:Panel>
</asp:Content>
