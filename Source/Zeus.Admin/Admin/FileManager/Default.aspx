<%@ Page Title="File Manager" Language="C#" MasterPageFile="~/Admin/Popup.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.FileManager.Default" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<link rel="stylesheet" href="../assets/css/shared.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	<link rel="stylesheet" href="../assets/css/tree.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	<link rel="stylesheet" href="../assets/css/thickbox.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	
	<script type="text/javascript" src="../assets/js/jquery.js"></script>
	<script type="text/javascript" src="../assets/js/plugins/jquery.simpleTree.js"></script>
	<script type="text/javascript" src="../assets/js/plugins/jquery.easing.js"></script>
	<script type="text/javascript" src="../assets/js/plugins/jquery.easing.compatibility.js"></script>
	<script type="text/javascript" src="../assets/js/plugins/jquery.dimensions.js"></script>
	<script type="text/javascript" src="../assets/js/plugins/jquery.contextMenu.js"></script>
	<script type="text/javascript" src="../assets/js/plugins/thickbox.js"></script>
	
	<script type="text/javascript">
		var simpleTreeCollection;

		function initContextMenu() {
			$("ul.simpleTree li span[data-type='Folder']").contextMenu({
				menu: 'myMenu'
			},
			function(action, el, pos) {
				switch (action) {
					case "upload":
						tb_show("Upload Image", "Upload.aspx?ParentPath=" + $(el).attr("data-path") + "&TB_iframe=true&height=50&width=200", false);
						break;
				}
			});
		}

		$(document).ready(function() {
			simpleTreeCollection = $('.simpleTree').simpleTree({
				autoclose: false,
				afterClick: function(node) {
					//top.preview.location.href = $("a:first", node).attr("href");
				},
				afterAjax: function() {
					initContextMenu();
				},
				animate: true,
				docToFolderConvert: true
			});

			initContextMenu();
		});
		
		function file_onClick(a) {
			var href;
			<% if (Request.QueryString["absoluteurls"] == "true") { %>
			href = $(a).attr('href');
			<% } else { %>
			href = $(a).attr("data-url");
			<% } %>
			selectFile(href);
			window.close();
			return false;
		}
		
		function selectFile(href) {
			<% if (Request.QueryString["direct"] == "true") { %>
			if (window.opener && window.opener.onQuickImageSelected) {
				window.opener.onQuickImageSelected(href);
				window.close();
			}
			<% } else { %>
			if (window.opener && window.opener.onFileSelected) {
				window.opener.onFileSelected(href);
				window.close();
			}
			<% } %>
		}
	
		$(document).ready(function() {
			$("span[data-type='File'] a").click(function() {
				return file_onClick(this);
			});
		});
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<div id="container">
		<h2>File Manager</h2>
		
		<p><b>Please expand the tree and click on a file to use it.</b>
		Alternatively, right-click on a folder to upload a new file into that folder.</p>
	
		<admin:FileTree runat="server" />
	</div>
	
	<ul id="myMenu" class="contextMenu">
		<li class="new">
			<a href="#upload">Upload</a>
		</li>
	</ul>
</asp:Content>