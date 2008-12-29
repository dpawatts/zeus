<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CheckoutDeliveryMethod.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.CheckoutDeliveryMethod" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSubNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1>Choose Delivery Method</h1>
	
	<p><strong>Welcome back, <%= this.Customer.FirstName %>.</strong><br />
	Please <a href="/assets/images/hamilton.jpg" target="_blank">view the map of Hamilton</a> to check the city delivery limits.</p>
	
	<asp:RadioButtonList runat="server" ID="rblDeliveryMethod" DataSourceID="cdsDeliveryTypes" DataTextField="Description" DataValueField="ID" />
	<zeus:ContentDataSource runat="server" ID="cdsDeliveryTypes" Axis="Descendant" Query="RootItem" OfType="Bermedia.Gibbons.Web.Items.BaseDeliveryType" />

	<asp:RequiredFieldValidator runat="server" ControlToValidate="rblDeliveryMethod" ErrorMessage="Please choose a delivery method" CssClass="warning" />
	
  <p>
		<sitdap:DynamicImageButton runat="server" ID="btnNext" TemplateName="Button" AlternateText="next" OnClick="btnNext_Click">
			<Layers>
				<sitdap:TextLayer Name="Text" Text="next" />
			</Layers>
		</sitdap:DynamicImageButton>
	</p>
	
	<h2>International Shipping Rates</h2>
	<asp:Table runat="server" ID="tblInternationalShippingRates" BorderWidth="0" CellPadding="5" CellSpacing="0" Width="672" />
  
  <p>For large quantity orders, oversized products or unusually heavy items, we reserve the right to pass 
  along additional costs for shipping. In addition, some countries may charge an additional handling fee 
  for delivery within that country. <strong>Rates are subject to change without notice.</strong></p>
</asp:Content>
