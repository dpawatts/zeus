<%@ Page Title="" Language="C#" MasterPageFile="~/Edit/PreviewFrame.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Zeus.Edit.Edit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
	<asp:Button runat="server" ID="btnSave" OnCommand="btnSave_Command" Text="Save" />
	<zeus:ItemEditor runat="server" ID="zeusItemEditor" />
</asp:Content>