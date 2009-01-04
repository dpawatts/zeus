<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="GiftCardThemes.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.GiftCardThemes" %>
<%@ Register TagPrefix="gibbons" TagName="PageNavigation" Src="../UserControls/PageNavigation.ascx" %>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:PageNavigation runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Gibbons Gift Cards</h1>
  
	<isis:TypedListView runat="server" DataSourceID="cdsThemes" GroupItemCount="3" DataItemTypeName="Bermedia.Gibbons.Web.Items.GiftCardTheme, Bermedia.Gibbons.Web">
		<LayoutTemplate>
			<table border="0" cellspacing="0" cellpadding="5">
				<asp:PlaceHolder runat="server" ID="groupPlaceholder" />
			</table>
		</LayoutTemplate>
		<GroupTemplate>
			<tr>
				<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
			</tr>
		</GroupTemplate>
		<ItemTemplate>
			<td valign="top">
				<sitdap:DynamicImage runat="server">
					<Layers>
						<sitdap:ImageLayer>
							<Source>
								<zeus:ZeusImageSource ContentID='<%# (Container.DataItem.Image != null) ? Container.DataItem.Image.ID : 0 %>' />
							</Source>
						</sitdap:ImageLayer>
					</Layers>
				</sitdap:DynamicImage>
				<br />
				<a href="gift-card-theme-purchase.aspx?id=<%# Container.DataItem.ID %>"><%# Container.DataItem.Title %></a>
			</td>
		</ItemTemplate>
	</isis:TypedListView>
	</table>
  <zeus:ContentDataSource runat="server" ID="cdsThemes" />
</asp:Content>