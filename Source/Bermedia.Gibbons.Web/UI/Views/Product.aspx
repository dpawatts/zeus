<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.Product" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1><%= this.CurrentItem.Brand.Title %> <%= this.CurrentItem.Title%></h1> 
			
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
		
		<asp:PlaceHolder runat="server" Visible='<%$ HasItems:AssociatedSizes %>'>
			<p>
				<strong>Available Sizes:</strong><br />
				<isis:DropDownList runat="server" ID="ddlSizes" DataSource='<%$ CurrentPage:AssociatedSizes %>' DataTextField="Title" DataValueField="ID" AppendDataBoundItems="true" RequiresDataBinding="true">
					<asp:ListItem Value="">Please select a size</asp:ListItem>
				</isis:DropDownList>
			</p>
		</asp:PlaceHolder>
		
		<asp:PlaceHolder runat="server" Visible='<%$ HasItems:AssociatedColours %>'>
			<p>
				<strong>Available Colors:</strong><br />
				<isis:DropDownList runat="server" ID="ddlColours" DataSource='<%$ CurrentPage:AssociatedColours %>' DataTextField="Title" DataValueField="ID" AppendDataBoundItems="true" RequiresDataBinding="true">
					<asp:ListItem Value="">Then select a color</asp:ListItem>
				</isis:DropDownList>
			</p>
		</asp:PlaceHolder>
		
		<isis:ConditionalMultiView runat="server" Value='<%$ Code:CurrentItem %>'>
			<isis:ConditionalView runat="server" Expression="it.SalePrice == null">
				<ItemTemplate>
					<h2><%# ((decimal) Eval("RegularPrice")).ToString("C2") %></h2>
				</ItemTemplate>
			</isis:ConditionalView>
			<isis:ConditionalView runat="server">
				<ItemTemplate>
					<h2><%# ((decimal) Eval("SalePrice")).ToString("C2") %> SALE</h2>
					<h2 class="oldPrice">was <%# ((decimal) Eval("RegularPrice")).ToString("C2") %></h2><br />
				</ItemTemplate>
			</isis:ConditionalView>
		</isis:ConditionalMultiView>
		
		<asp:PlaceHolder runat="server" Visible='<%$ Code:CurrentItem.Exclusive %>'>
			<strong>Gibbons Exclusive</strong><br />
		</asp:PlaceHolder>
		
		<asp:PlaceHolder runat="server" Visible='<%$ HasValue:FreeGiftProduct %>'>
			<a href="javascript:alert('todo');">Free gift with purchase</a>
		</asp:PlaceHolder>
		
		<p><asp:TextBox runat="server" size="3" Text="1" ID="txtQuantity" /> <strong>Quantity</strong></p>
		<sitdap:DynamicImageButton runat="server" ID="btnAddToCart" TemplateName="Button" AlternateText="add to cart" OnClick="btnAddToCart_Click">
			<Layers>
				<sitdap:TextLayer Name="Text" Text="+ add to cart" />
			</Layers>
		</sitdap:DynamicImageButton>
	</div>
</asp:Content>