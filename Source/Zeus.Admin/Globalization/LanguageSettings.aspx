<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LanguageSettings.aspx.cs" Inherits="Zeus.Admin.Globalization.LanguageSettings" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register TagPrefix="isis" Namespace="Isis.Web.UI.HtmlControls" Assembly="Isis" %>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
	<admin:ToolbarButton runat="server" ID="btnSave" Text="Save" ImageResourceName="Zeus.Admin.Assets.Images.Icons.tick.png" CssClass="positive" OnClick="btnSave_Click" />
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" ImageResourceName="Zeus.Admin.Assets.Images.Icons.cross.png" CssClass="negative" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<p><asp:CheckBox runat="server" ID="chkInheritSettings" /></p>
	
	<isis:FieldSet runat="server" Legend="Fallback Languages">
		<p>Pages in this part of the site may be made available in a language other than the one selected by the visitor. 
		This means that pages that have not yet been translated and published in the selected language can instead be temporarily 
		displayed in another language. Please note that this setting may cause mixed languges in navigation and listings, 
		which may be confusing for the visitor.</p>
		<br />
		
		<asp:Table runat="server" ID="tblFallbackLanguages" CssClass="tb" />
	</isis:FieldSet>
</asp:Content>
