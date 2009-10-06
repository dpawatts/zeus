<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LanguageOverview.aspx.cs" Inherits="Zeus.Admin.Globalization.LanguageOverview" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" ImageResourceName="Zeus.Admin.Assets.Images.Icons.cross.png" CssClass="negative" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<asp:Table runat="server" ID="tblPageTranslations" CssClass="tb languages" />
</asp:Content>
