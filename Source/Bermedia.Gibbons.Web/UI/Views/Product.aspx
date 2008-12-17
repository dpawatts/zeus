<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.Product" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<isis:TypedExtendedFormView runat="server" DataSourceID="cdsCurrentItem" DataItemTypeName="Bermedia.Gibbons.Web.Items.StandardProduct" DefaultMode="ReadOnly">
		<ItemTemplate>
			<h1><%# Container.DataItem.Brand.Title %> <%# Container.DataItem.Title %></h1> 
			
			<div id="productImage">
				<zeus:ItemDetailView runat="server" PropertyName="Image" />
				
				<isis:TypedListView runat="server" DataSource='<%# Container.DataItem.AssociatedColours %>' DataItemTypeName="Bermedia.Gibbons.Web.Items.ProductColour">
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
				<p><%# Container.DataItem.Description %></p>
				
				<asp:PlaceHolder runat="server" Visible='<%# Container.DataItem.AssociatedSizes.Count > 0 %>'>
					<p>
						<strong>Available Sizes:</strong><br />
						<asp:DropDownList runat="server" DataSource='<%# Container.DataItem.AssociatedSizes %>' DataTextField="Title" DataValueField="ID" AppendDataBoundItems="true">
							<asp:ListItem Value="">Please select a size</asp:ListItem>
						</asp:DropDownList>
					</p>
				</asp:PlaceHolder>
				
				<asp:PlaceHolder runat="server" Visible='<%# Container.DataItem.AssociatedColours.Count > 0 %>'>
					<p>
						<strong>Available Colors:</strong><br />
						<asp:DropDownList runat="server" DataSource='<%# Container.DataItem.AssociatedColours %>' DataTextField="Title" DataValueField="ID" AppendDataBoundItems="true">
							<asp:ListItem Value="">Then select a color</asp:ListItem>
						</asp:DropDownList>
					</p>
				</asp:PlaceHolder>
				
				<isis:ConditionalMultiView runat="server" Value='<%# Container.DataItem %>'>
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
				
				<asp:PlaceHolder runat="server" Visible='<%# Container.DataItem.Exclusive %>'>
					<strong>Gibbons Exclusive</strong><br />
				</asp:PlaceHolder>
				
				<asp:PlaceHolder runat="server" Visible='<%# Container.DataItem.FreeGiftProduct != null %>'>
					<a href="javascript:alert('todo');">Free gift with purchase</a>
				</asp:PlaceHolder>
				
				<p><asp:TextBox runat="server" size="3" Text="1" ID="txtQuantity" /> <strong>Quantity</strong></p>
				<sitdap:DynamicImage runat="server" TemplateName="Button" AlternateText="add to cart">
					<Layers>
						<sitdap:TextLayer Name="Text" Text="+ add to cart" />
					</Layers>
				</sitdap:DynamicImage>
			</div>
		</ItemTemplate>
	</isis:TypedExtendedFormView>
	
	<zeus:CurrentItemDataSource runat="server" ID="cdsCurrentItem" />
</asp:Content>