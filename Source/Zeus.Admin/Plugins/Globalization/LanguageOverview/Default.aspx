<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.Globalization.LanguageOverview.Default" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" Icon="Cross" CssClass="negative" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<asp:Table runat="server" ID="tblPageTranslations" CssClass="tb languages" />
</asp:Content>
