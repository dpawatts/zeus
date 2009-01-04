<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CheckoutPaymentDetails.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.CheckoutPaymentDetails" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1>Payment Details</h1>
	
	<h2>I am paying by credit card</h2>

	<asp:Panel runat="server" DefaultButton="btnNext">
		<table cellspacing="0" cellpadding="1" border="0">
			<tr>
				<td width="150"><asp:Label runat="server" AssociatedControlID="txtFirstName">Cardholder's first name</asp:Label></td>
				<td><asp:TextBox runat="server" ID="txtFirstName" Text='<%$ Code:this.CheckoutData.FirstName %>' /> <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" ErrorMessage="First name is required" Text="*" /></td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtLastName">Cardholder's last name</asp:Label></td>
				<td><asp:TextBox runat="server" ID="txtLastName" Text='<%$ Code:this.CheckoutData.LastName %>' /></td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="ddlCardType">Card Type</asp:Label></td>
				<td>
					<asp:DropDownList runat="server" ID="ddlCardType" SelectedValue='<%$ Code:this.CheckoutData.PaymentCardType %>' >
						<asp:ListItem>VISA</asp:ListItem>
						<asp:ListItem>MasterCard</asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="ddlExpiryDateMonth">Expiry Date</asp:Label></td>
				<td>
					<isis:DropDownList runat="server" ID="ddlExpiryDateMonth" SelectedValue='<%$ Code:this.CheckoutData.PaymentCardExpiryDate.Month %>' DataSource='<%# Isis.Linq.ExtendedEnumerable.MonthNames() %>' AppendDataBoundItems="true" RequiresDataBinding="true" DataTextField="Name" DataValueField="Month">
						<asp:ListItem Value="1" Text="" />
					</isis:DropDownList>
					<isis:DropDownList runat="server" ID="ddlExpiryDateYear" SelectedValue='<%$ Code:this.CheckoutData.PaymentCardExpiryDate.Year %>' DataSource='<%# System.Linq.Enumerable.Range(DateTime.Now.Year, 22) %>' AppendDataBoundItems="true" RequiresDataBinding="true">
						<asp:ListItem Value="1" Text="" />
					</isis:DropDownList>
					<asp:RequiredFieldValidator runat="server" ControlToValidate="ddlExpiryDateMonth" ErrorMessage="Expiry date month is required" Text="*" />
					<asp:RequiredFieldValidator runat="server" ControlToValidate="ddlExpiryDateYear" ErrorMessage="Expiry date year is required" Text="*" />
				</td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtCardNumber">Card Number</asp:Label></td>
				<td><asp:TextBox runat="server" ID="txtCardNumber" Text='<%$ Code:this.CheckoutData.PaymentCardNumber %>' /> <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCardNumber" ErrorMessage="Card Number is required" Text="*" /></td>
			</tr>
			<tr>
				<td><asp:Label runat="server" AssociatedControlID="txtCVV2">CVV2</asp:Label></td>
				<td><asp:TextBox runat="server" ID="txtCVV2" Text='<%$ Code:this.CheckoutData.PaymentCvv2 %>' /> <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCVV2" ErrorMessage="CVV2 is required" Text="*" /></td>
			</tr>
			<tr>
				<td colspan="2"><a onclick="alert('This is the three digit security code found on the back of your credit card.');" href="#">What's the CVV2 number?</a></td>
			</tr>
			<tr>
				<td></td>
				<td>
					<p><asp:ValidationSummary runat="server" ID="vlsSummary" DisplayMode="List" /></p>
					<br />
					
					<sitdap:DynamicImageButton runat="server" ID="btnNext" TemplateName="Button" AlternateText="next" OnClick="btnNext_Click">
						<Layers>
							<sitdap:TextLayer Name="Text" Text="next" />
						</Layers>
					</sitdap:DynamicImageButton>
				</td>
			</tr>
		</table>
	</asp:Panel>
	
	<br />
	
	<h2>I am paying by Corporate Account</h2>
	
	<p>Corporate Account holders who will fax in their orders should click "Next" below.</p>
	
	<table cellspacing="0" cellpadding="1" border="0">
		<tr>
			<td width="150"></td>
			<td>
				<sitdap:DynamicImageButton runat="server" ID="btnCorporateNext" TemplateName="Button" AlternateText="next" OnClick="btnCorporateNext_Click" CausesValidation="false">
					<Layers>
						<sitdap:TextLayer Name="Text" Text="next" />
					</Layers>
				</sitdap:DynamicImageButton>
			</td>
		</tr>
	</table>
</asp:Content>
