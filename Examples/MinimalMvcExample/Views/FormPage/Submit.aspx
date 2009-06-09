<%@ Page Language="C#" AutoEventWireup="true" Inherits="Zeus.Web.Mvc.ContentViewPage<Zeus.Templates.Mvc.Controllers.Forms.IFormPageViewData>" %>
<%@ Import Namespace="Zeus.Templates.Mvc.Html"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
			<h1><%= TypedViewData.CurrentPage.PageTitle %></h1>
			
			<h2>Form</h2>
			<%= TypedViewData.CurrentPage.Form.SubmitText %>
			
			<h2>Results</h2>
			<%= TempData["Results"] %>
    </div>
    </form>
</body>
</html>
