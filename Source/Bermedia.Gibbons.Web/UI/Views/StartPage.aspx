<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="StartPage.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.StartPage" %>
<%@ Register TagPrefix="gibbons" TagName="PageNavigation" Src="../UserControls/PageNavigation.ascx" %>
<asp:Content ContentPlaceHolderID="cphStyle" runat="server">
	#header {
		background-image:url(/assets/images/header-homepage.jpg);
		height:160px;
	}
	
	#headerNavigation {
		margin-top: 12px;
	}
	
	#loginNotice {
		margin-top: 115px;
	}
	
	#contentContainer {
		padding: 0;
		width: 761px;
	}
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:PageNavigation runat="server" />
	<%= this.CurrentItem.NavigationText %>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<%= this.CurrentItem.Text %>
</asp:Content>