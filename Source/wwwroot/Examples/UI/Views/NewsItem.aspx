<%@ Page Language="C#" MasterPageFile="~/Examples/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="NewsItem.aspx.cs" Inherits="Zeus.Examples.UI.Views.NewsItem" %>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<zeus:Displayer runat="server" PropertyName="Introduction" />
</asp:Content>