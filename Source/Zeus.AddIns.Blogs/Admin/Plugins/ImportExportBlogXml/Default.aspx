<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.AddIns.Blogs.Admin.Plugins.ImportExportBlogXml.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link rel="stylesheet" href="/assets/css/shared.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<h2>Import Atom XML</h2>
	
	<asp:CustomValidator id="cvImport" runat="server" CssClass="info validator" Display="Dynamic"/>
	<div class="upload">
		<p>
			<asp:FileUpload ID="uplImport" runat="server" />
			<asp:RequiredFieldValidator ID="rfvUpload" ControlToValidate="uplImport" runat="server" ErrorMessage="*" />
		</p>
		<p>
			<asp:Button ID="btnUploadImport" runat="server" Text="Import here" OnClick="btnUploadImport_Click" />
    </p>
	</div>
</asp:Content>