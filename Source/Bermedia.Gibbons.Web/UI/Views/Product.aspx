<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.Product" %>
<%@ Import Namespace="Bermedia.Gibbons.Web.Items"%>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1><%= CurrentItem.DisplayTitle %></h1> 
			
	<div id="productImage">
		<zeus:ItemDetailView runat="server" PropertyName="Image" />
		
		<table width="200" id="productColours">
			<% foreach (ProductColour productColour in CurrentItem.AssociatedColours) { %>
			<tr>
				<td width="30" height="30" style="background-color:#<%= productColour.HexRef %>">&nbsp;</td>
				<td style="padding-left: 10px"><%= productColour.Title %> </td>
			</tr>
			<tr>
				<td colspan="2">&nbsp;</td>
			</tr>
			<% } %>
		</table>
	</div>
	
	<div id="productDetails">
		<p><%= CurrentItem.Description %></p>
		
		<asp:PlaceHolder runat="server" Visible='<%$ Code:(this.CurrentItem.AssociatedSizes.Count > 0 && !(this.CurrentItem is Bermedia.Gibbons.Web.Items.FragranceBeautyProduct)) %>'>
			<p>
				<asp:PlaceHolder runat="server" Visible='<%$ Code:(this.CurrentItem.AssociatedSizes.Count == 1) %>'>
					<%= CurrentItem.AssociatedSizes[0].Title %>
				</asp:PlaceHolder>
				<asp:PlaceHolder runat="server" Visible='<%$ Code:(this.CurrentItem.AssociatedSizes.Count != 1) %>'>
					<strong>Available Sizes:</strong><br />
					<isis:DropDownList runat="server" ID="ddlSizes" DataSource='<%$ CurrentPage:AssociatedSizes %>' DataTextField="Title" DataValueField="ID" AppendDataBoundItems="true" RequiresDataBinding="true" AutoPostBack="true" OnSelectedIndexChanged="ddlSizes_SelectedIndexChanged">
						<asp:ListItem Value="">Please select a size</asp:ListItem>
					</isis:DropDownList>
					<asp:RequiredFieldValidator runat="server" ID="rfvSizes" ControlToValidate="ddlSizes" ErrorMessage="Please select a size" />
				</asp:PlaceHolder>
			</p>
		</asp:PlaceHolder>
		<%= CurrentItem.SubTitle %>
		
		<asp:PlaceHolder runat="server" Visible='<%$ Code:(this.CurrentItem.AssociatedColours.Count > 0 && !(this.CurrentItem is Bermedia.Gibbons.Web.Items.FragranceBeautyProduct)) %>'>
			<p>
			<asp:PlaceHolder runat="server" Visible='<%$ Code:(this.CurrentItem.AssociatedColours.Count == 1) %>'>
				<%= ((Bermedia.Gibbons.Web.Items.ProductColour) CurrentItem.AssociatedColours[0]).Title %>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" Visible='<%$ Code:(this.CurrentItem.AssociatedColours.Count != 1) %>'>
				<strong>Available Colors:</strong><br />
				<isis:DropDownList runat="server" ID="ddlColours" DataSource='<%$ CurrentPage:AssociatedColours %>' DataTextField="Title" DataValueField="ID" AppendDataBoundItems="true" RequiresDataBinding="true">
					<asp:ListItem Value="">Then select a color</asp:ListItem>
				</isis:DropDownList>
				<asp:RequiredFieldValidator runat="server" ID="rfvColours" ControlToValidate="ddlColours" ErrorMessage="Please select a color" />
			</asp:PlaceHolder>
			</p>
		</asp:PlaceHolder>
		
		<asp:PlaceHolder runat="server" ID="plcRegularPriceOnly" Visible="false">
			<h2 runat="server" id="h2Price" />
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="plcSalePrice" Visible="false">
			<h2 class="sale"><asp:Literal runat="server" ID="ltlSalePrice" /> SALE</h2>
			<h2 class="oldPrice">was <asp:Literal runat="server" ID="ltlOldPrice" /></h2><br />
		</asp:PlaceHolder>
		
		<asp:PlaceHolder runat="server" Visible='<%$ Code:CurrentItem.Exclusive %>'>
			<strong>Gibbons Exclusive</strong><br />
		</asp:PlaceHolder>
		
		<% if (CurrentItem.FreeGiftProduct != null) { %>
			<a href="<%= CurrentItem.FreeGiftProduct.Url %>" onclick="window.open($(this).attr('href'), 'FreeGift', 'width=500px,height=500px'); return false;">Free gift with purchase</a>
		<% } %>
		
		<p><asp:TextBox runat="server" size="3" Text="1" ID="txtQuantity" /> <strong>Quantity</strong></p>
		<sitdap:DynamicImageButton runat="server" ID="btnAddToCart" TemplateName="Button" AlternateText="add to cart" OnClick="btnAddToCart_Click">
			<Layers>
				<sitdap:TextLayer Name="Text" Text="+ add to cart" />
			</Layers>
		</sitdap:DynamicImageButton>
	</div>
</asp:Content>