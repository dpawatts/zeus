<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.ShoppingCart" %>
<%@ Register TagPrefix="gibbons" TagName="ProductListing" Src="../UserControls/ProductListing.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSubNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<h1>Shopping Cart</h1>

  <isis:TypedListView runat="server" ID="lsvShoppingCartItems" DataItemTypeName="Bermedia.Gibbons.Web.Items.ShoppingCartItem, Bermedia.Gibbons.Web" DataKeyNames="ID">
		<LayoutTemplate>
		  <p>The following items are in your shopping cart:</p>
  
			<table width="700" border="0" cellspacing="0" cellpadding="0">
				<tr>
					<th width="200" valign="top" style="border-bottom:#CCCCCC solid 1px;">Item</th>
					<th width="80" valign="top" style="border-bottom:#CCCCCC solid 1px;">Size</th>
					<th width="122" valign="top" style="border-bottom:#CCCCCC solid 1px;">Colour</th>
					<th width="62" valign="top" style="border-bottom:#CCCCCC solid 1px;">Qty</th>

					<th width="73" valign="top" style="border-bottom:#CCCCCC solid 1px;">Price</th>
					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">Is this a gift?</th>
					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">Gift Wrap</th>
				</tr>
				<tr>
					<td valign="top">&nbsp;</td>
					<td valign="top">&nbsp;</td>

					<td valign="top">&nbsp;</td>
					<td valign="top">&nbsp;</td>
					<td valign="top">&nbsp;</td>
					<td valign="top">&nbsp;</td>
					<td valign="top"><label></label></td>
				</tr>
				<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
				<tr>
					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">&nbsp;</th>
					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">&nbsp;</th>
					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">&nbsp;</th>
					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">&nbsp;</th>

					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">&nbsp;</th>
					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">&nbsp;</th>
					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">&nbsp;</th>
				</tr>

				<tr>
					<th height="25" valign="top" style="border-bottom:#CCCCCC solid 1px;">&nbsp;</th>
					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">&nbsp;</th>
					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">&nbsp;</th>

					<th valign="middle" style="border-bottom:#CCCCCC solid 1px;">Total</th>
					<th align="left" valign="middle" style="border-bottom:#CCCCCC solid 1px;"><asp:Literal runat="server" Text='<%$ Code:ShoppingCart.TotalPrice.ToString("C2") %>' /></th>
					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">&nbsp;</th>
					<th valign="top" style="border-bottom:#CCCCCC solid 1px;">&nbsp;</th>
				</tr>
				<tr>
					<td valign="top">&nbsp;</td>
					<td valign="top">&nbsp;</td>

					<td valign="top">&nbsp;</td>
					<td colspan="4" align="right" valign="top">
						<sitdap:DynamicImageButton runat="server" ID="btnUpdateQuantities" TemplateName="WhiteButton" AlternateText="update quantities" OnClick="btnUpdateQuantities_Click">
							<Layers>
								<sitdap:TextLayer Name="Text" Text="update quantities" />
							</Layers>
						</sitdap:DynamicImageButton>
					</td>
				</tr>
				<tr>
					<td valign="top">&nbsp;</td>
					<td valign="top">&nbsp;</td>
					<td valign="top">&nbsp;</td>
					<td colspan="4" align="right" valign="top">
						<sitdap:DynamicImageButton runat="server" ID="btnContinueShopping" TemplateName="WhiteButton" AlternateText="continue shopping" OnClick="btnContinueShopping_Click">
							<Layers>
								<sitdap:TextLayer Name="Text" Text="continue shopping" />
							</Layers>
						</sitdap:DynamicImageButton>
					</td>
				</tr>

				<tr>
					<td valign="top">&nbsp;</td>
					<td valign="top">&nbsp;</td>
					<td valign="top">&nbsp;</td>
					<td colspan="4" align="right" valign="top"><a href="/checkout-delivery-method.aspx">
						<sitdap:DynamicImage runat="server" TemplateName="Button" AlternateText="proceed to checkout">
							<Layers>
								<sitdap:TextLayer Name="Text" Text="proceed to checkout" />
							</Layers>
						</sitdap:DynamicImage>
					</a></td>
				</tr>
			</table>
		</LayoutTemplate>
		<ItemTemplate>
			<tr>
				<td valign="top"><a href="<%# Container.DataItem.Product.Url %>"><%# Container.DataItem.Product.Brand.Title %> <%# Container.DataItem.Product.Title %></a></td>

				<td valign="top"><%# (Container.DataItem.Size != null) ? Container.DataItem.Size.Title : string.Empty %></td>
				<td valign="top"><%# (Container.DataItem.Colour != null) ? Container.DataItem.Colour.Title : string.Empty %></td>
				<td valign="top">
					<asp:TextBox runat="server" ID="txtQuantity" Width="30px" Text='<%# Container.DataItem.Quantity %>' />
				</td>
				<td valign="top"><%# Container.DataItem.Price.ToString("C2") %></td>
				<td valign="top">
					<asp:CheckBox runat="server" ID="chkIsGift" Checked='<%# Container.DataItem.IsGift %>' />
				</td>
				<td valign="top">
					<isis:DropDownList runat="server" ID="ddlGiftWrapTypes" DataSourceID="cdsGiftWrapTypes" DataValueField="ID" DataTextField="Title" AppendDataBoundItems="true" style="width:auto" SelectedValue='<%# (Container.DataItem.GiftWrapType != null) ? Container.DataItem.GiftWrapType.ID.ToString() : string.Empty %>'>
						<asp:ListItem Selected="True" Value="" style="color:#CCCCCC">No gift wrap</asp:ListItem>
					</isis:DropDownList>
					<zeus:ContentDataSource runat="server" ID="cdsGiftWrapTypes" OfType="Bermedia.Gibbons.Web.Items.GiftWrapType" Query="RootItem" Axis="Descendant" />
				</td>
			</tr>
		</ItemTemplate>
		<EmptyDataTemplate>
			<p><b>There are currently no items in your shopping cart.</b></p>
		</EmptyDataTemplate>
  </isis:TypedListView>
   
  <gibbons:ProductListing runat="server" DataSourceID="cdsRecommendedProducts" HeaderText="You might also like" />
	<zeus:ContentDataSource runat="server" ID="cdsRecommendedProducts" OfType="Bermedia.Gibbons.Web.Items.StandardProduct" Query="RootItem" Axis="Descendant" Where="ID == @ProductID" Select="Recommendations">
		<WhereParameters>
			<asp:QueryStringParameter Name="ProductID" QueryStringField="add" Type="Int32" DefaultValue="-1" />
		</WhereParameters>
	</zeus:ContentDataSource>
</asp:Content>