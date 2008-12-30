<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="FragranceBeautyBrands.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.FragranceBeautyBrands" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<%@ Register TagPrefix="gibbons" TagName="ProductListing" Src="../UserControls/ProductListing.ascx" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Shop by Brand</h1>
	<br />
	
  <table width="700" border="0" cellpadding="0" cellspacing="0" id="brands">
    <tr>
			<isis:TypedRepeater runat="server" DataSourceID="cdsChildren" DataItemTypeName="Bermedia.Gibbons.Web.Items.FragranceBeautyCategory, Bermedia.Gibbons.Web">
				<ItemTemplate>
					<td width="33%" valign="top">
						<h2><%# Container.DataItem.Title %></h2>
						<ul>
							<isis:TypedRepeater runat="server" DataSource='<%# Container.DataItem.GetChildren<Bermedia.Gibbons.Web.Items.FragranceBeautyBrandCategory>().OrderBy(c => c.Title) %>' DataItemTypeName="Bermedia.Gibbons.Web.Items.FragranceBeautyBrandCategory, Bermedia.Gibbons.Web">
								<ItemTemplate>
									<li><a href="<%# Container.DataItem.Url %>"><%# Container.DataItem.Title %></a></li>
								</ItemTemplate>
							</isis:TypedRepeater>
						</ul>
					</td>
				</ItemTemplate>
			</isis:TypedRepeater>
			<zeus:ContentDataSource runat="server" ID="cdsChildren" OfType="Bermedia.Gibbons.Web.Items.FragranceBeautyCategory, Bermedia.Gibbons.Web" />
  </table>
</asp:Content>