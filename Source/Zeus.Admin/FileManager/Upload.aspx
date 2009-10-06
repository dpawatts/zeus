<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Popup.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="Zeus.Admin.FileManager.Upload" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
	<asp:FileUpload runat="server" ID="uplFile" CssClass="required" /><br />
	<asp:Button runat="server" ID="btnUploadFile" Text="Upload" OnClick="btnUploadFile_Click" OnClientClick="tb_remove();" />
</asp:Content>