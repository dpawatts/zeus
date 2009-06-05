<%@ Page Language="C#" AutoEventWireup="true" Inherits="Zeus.Web.Mvc.ContentViewPage<Zeus.Examples.MinimalMvcExample.Controllers.ICustomPageViewData>" %>
<%@ Import Namespace="Zeus"%>
<%@ Import Namespace="Zeus.Globalization.ContentTypes"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
			<h1><%= TypedViewData.CurrentPage.PageTitle %></h1>
			
			<h2>Other Languages</h2>
			<ul>
				<% foreach (Language language in Zeus.Context.Current.LanguageManager.GetAvailableLanguages().Where(l => l.Name != TypedViewData.CurrentPage.Language)) { %>
				<% ContentItem translation = Zeus.Context.Current.LanguageManager.GetTranslation((ContentItem) Model, language.Name); %>
				<% if (translation != null) { %>
				<li><a href="<%= translation.Url %>"><%= language.Title %></a></li>
				<% } %>
				<% } %>
			</ul>
			
			<h2>Content</h2>
			<%= TypedViewData.CurrentPage.Content %>
			
			<h2>Search Results</h2>
			<% foreach (var searchResult in TypedViewData.SearchResults) { %>
			<%= searchResult.Title %><br />
			<% } %>
    </div>
    </form>
</body>
</html>
