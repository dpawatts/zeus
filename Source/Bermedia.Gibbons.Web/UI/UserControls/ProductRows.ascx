<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductRows.ascx.cs" Inherits="Bermedia.Gibbons.Web.UI.UserControls.ProductRows" %>
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
						<strong><%# Container.DataItem.DisplayTitle %></strong><br />
						<asp:PlaceHolder runat="server" Visible='<%# !string.IsNullOrEmpty(Container.DataItem.SubTitle) %>'>
							<%# Container.DataItem.SubTitle %><br />
						</asp:PlaceHolder>

						<%# (Container.DataItem.SalePrice == null) ?
									Container.DataItem.RegularPrice.ToString("C2") + "<br />" :
									"<span class=\"oldPrice\">" + Container.DataItem.RegularPrice.ToString("C2") + "</span><br /><span class=\"sale\">" + Container.DataItem.SalePrice.Value.ToString("C2") + " SALE</span><br />" %>
						
						<asp:PlaceHolder runat="server" Visible='<%# Container.DataItem.AssociatedColours.Count > 1 %>'>
							More Colours<br />
						</asp:PlaceHolder>
					</a>
					
					<asp:PlaceHolder runat="server" Visible='<%# Container.DataItem.Exclusive %>'>
						Gibbons' Exclusive<br />
					</asp:PlaceHolder>
					
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