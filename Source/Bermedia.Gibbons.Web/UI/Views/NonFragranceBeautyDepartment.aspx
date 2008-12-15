<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="NonFragranceBeautyDepartment.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.NonFragranceBeautyDepartment" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
	#contentContainer {
		padding: 0;
	}
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContent" runat="server">
	<%= this.CurrentItem.Text %>
</asp:Content>