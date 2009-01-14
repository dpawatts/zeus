<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateRange.ascx.cs" Inherits="Bermedia.Gibbons.Web.Plugins.Reports.DateRange" %>
<asp:DropDownList runat="server" id="ddlStartDay" cssclass="discreet" onselectedindexchanged="DateRange_ValueChanged"/>
<asp:DropDownList runat="server" id="ddlStartMonth" cssclass="discreet" onselectedindexchanged="DateRange_ValueChanged"/>
<asp:DropDownList runat="server" id="ddlStartYear" cssclass="discreet" onselectedindexchanged="DateRange_ValueChanged"/>
and
<asp:DropDownList runat="server" id="ddlEndDay" cssclass="discreet" onselectedindexchanged="DateRange_ValueChanged"/>
<asp:DropDownList runat="server" id="ddlEndMonth" cssclass="discreet" onselectedindexchanged="DateRange_ValueChanged"/>
<asp:DropDownList runat="server" id="ddlEndYear" cssclass="discreet" onselectedindexchanged="DateRange_ValueChanged"/>
<asp:Button ID="Button1" runat="server" cssclass="discreet" text="View"/>