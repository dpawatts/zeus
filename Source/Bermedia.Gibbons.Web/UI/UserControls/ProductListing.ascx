<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductListing.ascx.cs" Inherits="Bermedia.Gibbons.Web.UI.UserControls.ProductListing" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Zeus" %>

<h1 runat="server" id="h1Header" />

<isis:TypedListView runat="server" ID="lsvProducts" DataItemTypeName="Bermedia.Gibbons.Web.Items.StandardProduct, Bermedia.Gibbons.Web">
	<LayoutTemplate>
		<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
	</LayoutTemplate>
	<ItemTemplate>
		<div class="categoryProduct">
			<div class="image">
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
			</div>
		
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
		</div>
	</ItemTemplate>
</isis:TypedListView>