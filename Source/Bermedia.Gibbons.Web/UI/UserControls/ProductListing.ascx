<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductListing.ascx.cs" Inherits="Bermedia.Gibbons.Web.UI.UserControls.ProductListing" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Zeus" %>

<h1 runat="server" id="h1Header" />

<asp:Label runat="server" ID="ltlPageLinks" Visible="false" style="float:right;height:20px;margin-bottom:10px;" />
<br style="clear:both" />
		
<isis:TypedListView runat="server" ID="lsvProducts" DataItemTypeName="Bermedia.Gibbons.Web.UI.UserControls.ProductGroup, Bermedia.Gibbons.Web">
	<LayoutTemplate>
		<table class="productListing">
			<asp:PlaceHolder runat="server" ID="groupPlaceholder" />
		</table>
	</LayoutTemplate>
	<GroupTemplate>
		<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
	</GroupTemplate>
	<ItemTemplate>
		<isis:TypedListView runat="server" DataSource='<%# Container.DataItem.Products %>' DataItemTypeName="Bermedia.Gibbons.Web.Items.StandardProduct, Bermedia.Gibbons.Web">
			<LayoutTemplate>
				<tr>
					<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
				</tr>
			</LayoutTemplate>
			<ItemTemplate>
				<td class="image">
					<a href="<%# Container.DataItem.Url %>"><sitdap:DynamicImage runat="server">
						<Layers>
							<sitdap:ImageLayer>
								<Source>
									<zeus:ZeusImageSource ContentID='<%# (Container.DataItem.Image != null) ? Container.DataItem.Image.ID : 0 %>' />
								</Source>
								<AlternateSource>
									<sitdap:FileImageSource FileName="~/Assets/Images/no-image.jpg" />
								</AlternateSource>
								<Filters>
									<sitdap:ResizeFilter Width="125" Height="125" />
								</Filters>
							</sitdap:ImageLayer>
						</Layers>
					</sitdap:DynamicImage></a>
				</td>
			</ItemTemplate>
		</isis:TypedListView>
		
		<isis:TypedListView runat="server" DataSource='<%# Container.DataItem.Products %>' DataItemTypeName="Bermedia.Gibbons.Web.Items.StandardProduct, Bermedia.Gibbons.Web">
			<LayoutTemplate>
				<tr>
					<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
				</tr>
			</LayoutTemplate>
			<ItemTemplate>
				<td class="details">
					<a href="<%# Container.DataItem.Url %>">
						<strong><%# Container.DataItem.Brand.Title %> <%# Container.DataItem.Title %></strong><br />
						
						<isis:ConditionalMultiView runat="server" Value='<%# Container.DataItem %>'>
							<isis:ConditionalView runat="server" Expression="it.SalePrice == null">
								<ItemTemplate>
									<%# ((decimal) Eval("RegularPrice")).ToString("C2") %><br />
								</ItemTemplate>
							</isis:ConditionalView>
							<isis:ConditionalView runat="server">
								<ItemTemplate>
									<span class="oldPrice"><%# ((decimal) Eval("RegularPrice")).ToString("C2") %></span><br />
									<span class="sale"><%# ((decimal) Eval("SalePrice")).ToString("C2") %> SALE</span><br />
								</ItemTemplate>
							</isis:ConditionalView>
						</isis:ConditionalMultiView>
						
						<asp:PlaceHolder runat="server" Visible='<%# Container.DataItem.AssociatedColours.Count > 1 %>'>
							More Colours<br />
						</asp:PlaceHolder>
					</a>
					
					<asp:PlaceHolder runat="server" Visible='<%# Container.DataItem.FreeGiftProduct != null %>'>
						Free gift with purchase
					</asp:PlaceHolder>
				</td>
			</ItemTemplate>
		</isis:TypedListView>
	</ItemTemplate>
	<EmptyDataTemplate>
		<p><strong>No products match your search term. Please try again.</strong></p>
	</EmptyDataTemplate>
</isis:TypedListView>