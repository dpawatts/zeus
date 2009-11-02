<%@ Page Title="File Manager" Language="C#" MasterPageFile="~/Admin/Popup.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.FileManager.Default" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
		var simpleTreeCollection;
		
		function showUploadDialog(path) {
			tb_show("Upload Image", "Upload.aspx?ParentPath=" + path + "&TB_iframe=true&height=50&width=200", false);
		}
		
		function createContextMenu(span) {
			var id = span.attr("data-id");
			jQuery("#contextMenu" + id).remove(); // Remove existing menu
			var ul = jQuery("<ul></ul>").attr("id", "contextMenu" + id).addClass("contextMenu");
			var li = jQuery("<li></li>");
			var a = jQuery("<a></a>").text("Upload")
				.css({ "background-image" : "url(/admin/assets/images/icons/arrow_up.png)" })
				.attr("href", "#")
				.attr("onclick", "showUploadDialog('" + span.attr("data-path") + "')");
			li.append(a);
			ul.append(li);
			jQuery('#contextMenus').append(ul);
			return ul;
		}
		
		function initContextMenu() {
			jQuery("ul.simpleTree li span[data-type='Folder']").each(function(i) {
				jQuery(this).contextMenu(
					{ menuCallback: createContextMenu },
					function(action, el, pos) {
						
					}
				);
			});
		}

		jQuery(document).ready(function() {
			simpleTreeCollection = jQuery('.simpleTree').simpleTree({
				autoclose: false,
				afterClick: function(node) {
					//top.preview.location.href = $("a:first", node).attr("href");
				},
				afterAjax: function() {
					initContextMenu();
					setupLinks();
				},
				animate: true,
				docToFolderConvert: true
			});

			initContextMenu();
		});
		
		function file_onClick(a) {
			var href;
			<% if (Request.QueryString["absoluteurls"] == "true") { %>
			href = jQuery(a).attr('href');
			<% } else { %>
			href = jQuery(a).attr("data-url");
			<% } %>
			<% if (Request.QueryString["destinationType"] != "image") { %>
			href = "~/link/" + jQuery(a).parent().attr("data-id");
			<% } %>
			selectFile(href);
			window.close();
			return false;
		}
		
		function selectFile(href) {
			if (window.opener && window.opener.onFileSelected) {
				window.opener.onFileSelected(href);
				window.close();
			}
		}
		
		function setupLinks() {
		<% if (Request.QueryString["destinationType"] == "image") { %>
			jQuery("span[data-type='File'] a").click(function() {
				return file_onClick(this);
			});
			jQuery("span[data-type='Image'] a").click(function() {
				return file_onClick(this);
			});
		<% } else { %>
			jQuery("ul.simpleTree span a").click(function() {
				return file_onClick(this);
			});
		<% } %>
		}
		
		jQuery(document).ready(function() {
			setupLinks();
		});
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<div id="container">
		<h2>File Manager</h2>
		
		<p><b>Please expand the tree and click on a file to use it.</b>
		Alternatively, right-click on a folder to upload a new file into that folder.</p>
	
		<admin:FileTree runat="server" ID="ftrFileTree" />
	</div>
	
	<div id="contextMenus"></div>
</asp:Content>