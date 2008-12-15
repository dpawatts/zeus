<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.Category" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Zeus" %>
<asp:Content ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1><zeus:ItemDetailView runat="server" PropertyName="Title" /></h1>

	<div id="categoryImage">
		<sitdap:DynamicImage runat="server">
			<Layers>
				<sitdap:ImageLayer>
					<Source>
						<sitdap:ZeusImageSource PropertyName="Image" ContentID='<%$ CurrentPage:ID %>' />
					</Source>
					<Filters>
						<sitdap:ResizeFilter Width="300" Mode="UseWidth" />
					</Filters>
				</sitdap:ImageLayer>
			</Layers>
		</sitdap:DynamicImage>
	</div>
	
	<isis:TypedListView runat="server" DataSourceID="cdsChildren" DataItemTypeName="Bermedia.Gibbons.Web.Items.StandardProduct">
		<LayoutTemplate>
			<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
		</LayoutTemplate>
		<ItemTemplate>
			<div class="categoryProduct">
				<a href="<%# Container.DataItem.Url %>">
					<sitdap:DynamicImage runat="server">
						<Layers>
							<sitdap:ImageLayer>
								<Source>
									<sitdap:ZeusImageSource PropertyName="Image" ContentID='<%# Container.DataItem.ID %>' />
								</Source>
								<AlternateSource>
									<sitdap:FileImageSource FileName="~/Assets/Images/no-image.jpg" />
								</AlternateSource>
								<Filters>
									<sitdap:ResizeFilter Width="125" Mode="UseWidth" />
								</Filters>
							</sitdap:ImageLayer>
						</Layers>
					</sitdap:DynamicImage>
				</a>
				<br />
			
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
	<zeus:ContentDataSource runat="server" ID="cdsChildren" OfType="Bermedia.Gibbons.Web.Items.StandardProduct" />
</asp:Content>