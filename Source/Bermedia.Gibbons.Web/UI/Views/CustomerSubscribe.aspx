<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CustomerSubscribe.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.CustomerSubscribe" %>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Email Exclusives</h1>
	
	<asp:PlaceHolder runat="server" ID="plcForm">		
		<p>Please click the button below to receive exclusive offers by e-mail from Gibbons.bm:</p>

		<sitdap:DynamicImageButton runat="server" ID="btnSubscribe" OnClick="btnSubscribe_Click" TemplateName="Button" AlternateText="subscribe">
			<Layers>
				<sitdap:TextLayer Name="Text" Text="subscribe" />
			</Layers>
		</sitdap:DynamicImageButton>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder runat="server" ID="plcConfirmation" Visible="false">
		<p>Thank you. You should receive a confirmation e-mail shortly.</p>
		
		<a href="<%= this.CurrentItem.Url %>">
			<sitdap:DynamicImage runat="server" TemplateName="Button" AlternateText="my account">
				<Layers>
					<sitdap:TextLayer Name="Text" Text="my account" />
				</Layers>
			</sitdap:DynamicImage>
		</a>
	</asp:PlaceHolder>
</asp:Content>
