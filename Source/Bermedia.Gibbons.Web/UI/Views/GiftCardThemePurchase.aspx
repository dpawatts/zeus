<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="GiftCardThemePurchase.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.GiftCardThemePurchase" %>
<%@ Register TagPrefix="gibbons" TagName="PageNavigation" Src="../UserControls/PageNavigation.ascx" %>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:PageNavigation runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Gibbons Gift Cards</h1>
  
	<p>Please enter the name of the person you would like to give a gift card to, and choose an amount:</p>
	
	<table border="0" cellpadding="1" cellspacing="0">
		<tr>
			<td><asp:Label runat="server" AssociatedControlID="txtName" Text="Name" /> <span class="error">*</span></td>
			<td><asp:TextBox runat="server" ID="txtName" MaxLength="500" Width="230" /> <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName" Text="*" ErrorMessage="Name is required" /></td>
		</tr>
		<tr>
			<td><asp:Label runat="server" AssociatedControlID="txtAmount" Text="Amount ($)" /> <span class="error">*</span></td>
			<td><asp:TextBox runat="server" ID="txtAmount" MaxLength="5" Width="50" /> <asp:RequiredFieldValidator runat="server" ID="rfvAmount" ControlToValidate="txtAmount" Text="*" ErrorMessage="Amount is required" /></td>
		</tr>
		<tr>
			<td></td>
			<td>
				<asp:ValidationSummary runat="server" ID="vlsSummary" />
				
				<sitdap:DynamicImageButton runat="server" ID="btnBuy" TemplateName="Button" AlternateText="add to cart" OnClick="btnBuy_Click">
					<Layers>
						<sitdap:TextLayer Name="Text" Text="add to cart" />
					</Layers>
				</sitdap:DynamicImageButton>
		</tr>
	</table>
	
	<p><strong>Delivery Options</strong><br />
	Your order will be <strong>FILLED WITHIN 24 TO 48 HOURS</strong>. You can collect your package from our Church Street
	collection point with a photo ID, or we’ll deliver it right to you in Hamilton or island – wide for a small fee.</p>
	
	<p><strong>Gift cards may be used at the following stores in Hamilton:</strong></p>
	<ul>
		<li>25 Reid</li>
		<li>Nine West</li>
		<li>MAC</li>
		<li>Gibbons Company Limited</li>
	</ul>
</asp:Content>