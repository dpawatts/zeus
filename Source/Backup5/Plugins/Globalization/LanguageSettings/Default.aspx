<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.Globalization.LanguageSettings.Default" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
	<admin:ToolbarButton runat="server" ID="btnSave" Text="Save" Icon="Tick" CssClass="positive" OnClick="btnSave_Click" />
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" Icon="Cross" CssClass="negative" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<ext:ResourceManager runat="server" Theme="Gray" AjaxViewStateMode="Enabled" />
	<p><asp:CheckBox runat="server" ID="chkInheritSettings" /></p>
	
	<ext:FieldSet runat="server" Legend="Fallback Languages">
		<Content>
			<p>Pages in this part of the site may be made available in a language other than the one selected by the visitor. 
			This means that pages that have not yet been translated and published in the selected language can instead be temporarily 
			displayed in another language. Please note that this setting may cause mixed languges in navigation and listings, 
			which may be confusing for the visitor.</p>
			<br />
			
			<asp:Table runat="server" ID="tblFallbackLanguages" CssClass="tb" />
		</Content>
	</ext:FieldSet>
</asp:Content>
