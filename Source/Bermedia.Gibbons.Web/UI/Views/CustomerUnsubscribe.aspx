<%@ Page Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CustomerUnsubscribe.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.CustomerUnsubscribe" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
	<link rel="stylesheet" href="/assets/css/myaccount.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1>Unsubscribe From Email Exclusives</h1>
	
	<asp:PlaceHolder runat="server" ID="plcForm">
		<p>Please click the button below to remove your newsletter subscription:</p>

		<sitdap:DynamicImageButton runat="server" ID="btnUnsubscribe" OnClick="btnUnsubscribe_Click" TemplateName="Button" AlternateText="unsubscribe">
			<Layers>
				<sitdap:TextLayer Name="Text" Text="unsubscribe" />
			</Layers>
		</sitdap:DynamicImageButton>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder runat="server" ID="plcConfirmation" Visible="false">
		<p>Thank you. You have been unsubscribed from our email exclusives.</p>
		
		<a href="<%= this.CurrentItem.Url %>">
			<sitdap:DynamicImage runat="server" TemplateName="Button" AlternateText="my account">
				<Layers>
					<sitdap:TextLayer Name="Text" Text="my account" />
				</Layers>
			</sitdap:DynamicImage>
		</a>
	</asp:PlaceHolder>
</asp:Content>