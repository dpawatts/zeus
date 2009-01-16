<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.Product" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1><%= this.CurrentItem.DisplayTitle %></h1> 
			
	<div id="productImage">
		<zeus:ItemDetailView runat="server" PropertyName="Image" />
		
		<isis:TypedListView runat="server" DataSource='<%$ CurrentPage:AssociatedColours %>' DataItemTypeName="Bermedia.Gibbons.Web.Items.ProductColour, Bermedia.Gibbons.Web">
			<LayoutTemplate>
				<table width="200" id="productColours">
					<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
				</table>
			</LayoutTemplate>
			<ItemTemplate>
				<tr>
					<td width="30" height="30" style="background-color:<%# Container.DataItem.HexRef %>">&nbsp;</td>
					<td style="padding-left: 10px"><%# Container.DataItem.Name %></td>
				</tr>
				<tr>
					<td colspan="2">&nbsp;</td>
				</tr>
			</ItemTemplate>
		</isis:TypedListView>
	</div>
	
	<div id="productDetails">
		<p><%= this.CurrentItem.Description %></p>
		
		<asp:PlaceHolder runat="server" Visible='<%$ Code:(this.CurrentItem.AssociatedSizes.Count > 0 && !(this.CurrentItem is Bermedia.Gibbons.Web.Items.FragranceBeautyProduct)) %>'>
			<p>
				<asp:PlaceHolder runat="server" Visible='<%$ Code:(this.CurrentItem.AssociatedSizes.Count == 1) %>'>
					<%= this.CurrentItem.AssociatedSizes[0].Title %>
				</asp:PlaceHolder>
				<asp:PlaceHolder runat="server" Visible='<%$ Code:(this.CurrentItem.AssociatedSizes.Count != 1) %>'>
					<strong>Available Sizes:</strong><br />
					<isis:DropDownList runat="server" ID="ddlSizes" DataSource='<%$ CurrentPage:AssociatedSizes %>' DataTextField="Title" DataValueField="ID" AppendDataBoundItems="true" RequiresDataBinding="true">
						<asp:ListItem Value="">Please select a size</asp:ListItem>
					</isis:DropDownList>
					<asp:RequiredFieldValidator runat="server" ID="rfvSizes" ControlToValidate="ddlSizes" ErrorMessage="Please select a size" />
				</asp:PlaceHolder>
			</p>
		</asp:PlaceHolder>
		<%= this.CurrentItem.SubTitle %>
		
		<asp:PlaceHolder runat="server" Visible='<%$ Code:(this.CurrentItem.AssociatedColours.Count > 0 && !(this.CurrentItem is Bermedia.Gibbons.Web.Items.FragranceBeautyProduct)) %>'>
			<p>
			<asp:PlaceHolder runat="server" Visible='<%$ Code:(this.CurrentItem.AssociatedColours.Count == 1) %>'>
				<%= ((Bermedia.Gibbons.Web.Items.ProductColour) this.CurrentItem.AssociatedColours[0]).Title %>
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
		
		<% if (this.CurrentItem.SalePrice == null) { %>
			<h2><%= this.CurrentItem.RegularPrice.ToString("C2") %></h2>
		<% } else { %>
			<h2><%= this.CurrentItem.SalePrice.Value.ToString("C2")%> SALE</h2>
			<h2 class="oldPrice">was <%= this.CurrentItem.RegularPrice.ToString("C2")%></h2><br />
		<% } %>
		
		<asp:PlaceHolder runat="server" Visible='<%$ Code:CurrentItem.Exclusive %>'>
			<strong>Gibbons Exclusive</strong><br />
		</asp:PlaceHolder>
		
		<% if (this.CurrentItem.FreeGiftProduct != null) { %>
			<a href="<%= this.CurrentItem.FreeGiftProduct.Url %>" onclick="window.open($(this).attr('href'), 'FreeGift', 'width=500px,height=500px'); return false;">Free gift with purchase</a>
		<% } %>
		
		<p><asp:TextBox runat="server" size="3" Text="1" ID="txtQuantity" /> <strong>Quantity</strong></p>
		<sitdap:DynamicImageButton runat="server" ID="btnAddToCart" TemplateName="Button" AlternateText="add to cart" OnClick="btnAddToCart_Click">
			<Layers>
				<sitdap:TextLayer Name="Text" Text="+ add to cart" />
			</Layers>
		</sitdap:DynamicImageButton>
	</div>
</asp:Content>