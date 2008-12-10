<%@ Page Language="C#" MasterPageFile="PreviewFrame.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Zeus.Admin.Edit" ValidateRequest="false" %>
<asp:Content runat="server" ContentPlaceHolderID="Head">
	<link rel="stylesheet" href="assets/css/edit.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<asp:Button runat="server" ID="btnSave" OnCommand="btnSave_Command" Text="Save" CssClass="save" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<asp:ValidationSummary runat="server" CssClass="validationSummary" />
	<zeus:ItemEditView runat="server" ID="zeusItemEditView" />
</asp:Content>