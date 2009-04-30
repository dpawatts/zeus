<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeus.Admin.Navigation.Tree" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title>Navigation</title>
	
	<script type="text/javascript">
		zeusnav.memorize = function(selected, action) {
			window.top.zeus.memorize(selected, action);
		}
		
		var simpleTreeCollection;

		function createContextMenu(span) {
			var id = span.attr("data-id");
			$("#contextMenu" + id).remove(); // Remove existing menu
			var ul = $("<ul></ul>").attr("id", "contextMenu" + id).addClass("contextMenu");
			var menuItems = span.attr("data-contextmenuitems").split("|");
			for (var i = 0; i < menuItems.length; i++) {
				// 0: Name
				// 1: Enabled?
				// 2: CssClass
				var menuItemParts = menuItems[i].split("`");
				
				var actionPlugin = actionPlugins[menuItemParts[0]];
				var enableCondition = menuItemParts[1];
				if (actionPlugin.EnableCondition)
					enableCondition += " && " + actionPlugin.EnableCondition;
				
				var li = $("<li></li>");
				var a = $("<a></a>").text(actionPlugin.Text)
					.css({ "background-image": "url(" + actionPlugin.ImageUrl + ")" })
					.attr("target", actionPlugin.Target)
					.attr("href", "#")
					; //.attr("data-path", span.attr("data-path"));
				if (eval(enableCondition)) {
					var memory, action;
					if (window.top.zeus) {
						memory = window.top.zeus.getMemory();
						action = window.top.zeus.getAction();
					}
					var clickAction = actionPlugin.PageUrl
						.replace("{selected}", span.attr("data-path"))
						.replace("{memory}", memory)
						.replace("{action}", action);
					a = a.attr("href", clickAction);
				}
				else {
					li = li.addClass("disabled");
				}
				li = li.addClass(menuItemParts[2]);
				li.append(a);
				ul.append(li);
			}
			$('#contextMenus').append(ul);
			return ul;
		}

		function changePreviewUrl(url) {
			top.preview.location.href = url;
		}

		function initContextMenu() {
			<asp:PlaceHolder runat="server" ID="plcTooltipsJavascript">
			$('ul.simpleTree li span a').each(function() {
				var splitTitle = $(this).attr("data-title").split("`");
				$(this).qtip({
					content: "Content Type: " + splitTitle[0] + "<br />"
						+ "Created: " + splitTitle[1] + "<br />"
						+ "Updated: " + splitTitle[2] + "<br />"
						+ "Content ID: " + splitTitle[3],
					show: {
						solo: true,
						when: { event: 'mouseover' }
					},
					//hide: 'click',
					style: { 'background-color': '#ffffe1' },
					position: {
						corner: { target: 'bottomLeft' },
						adjust: { x: 20, y: 60 },
						container: $(window.top.document.body)
					}
				});
			});
			</asp:PlaceHolder>

			$("ul.simpleTree li span").each(function(i) {
				$(this).contextMenu(
					{ menuCallback: createContextMenu },
					function(action, el, pos) {
						
					}
				);
			});
		}

		$(document).ready(function() {
			simpleTreeCollection = $('.simpleTree').simpleTree({
				autoclose: false,
				afterClick: function(node) {
					top.preview.location.href = $("a:first", node).attr("href");
					//alert("text-"+$('span:first',node).text());
				},
				afterDblClick: function(node) {
					//alert("text-"+$('span:first',node).text());
				},
				moving: function(destination, source) {
					return confirm("Are you sure you wish to move this item?");
				},
				afterMove: function(destination, source, pos) {
					var sourcePath = $("span:first", source).attr('data-path');
					var destinationPath = $("span:first", destination).attr('data-path');
					top.preview.location.href = "/admin/move.aspx?selected=" + sourcePath + "&destination=" + destinationPath + "&pos=" + pos;
				},
				afterAjax: function() {
					initContextMenu();
				},
				animate: true,
				docToFolderConvert: true
			});

			initContextMenu();
		});
	</script>
</head>
<body style="height:100%">
	<div id="outerContainer">
		<div id="container">
			<admin:Tree runat="server" />
		</div>

		<div id="contextMenus"></div>
		
		<form runat="server">
			<asp:PlaceHolder runat="server" ID="plcLanguages">
				<div id="languages">
					<p>Show page tree:<br />
					<admin:LanguageDropDownList runat="server" ID="ddlLanguages" OnLanguageChanged="ddlLanguages_LanguageChanged" /></p>
				</div>
			</asp:PlaceHolder>
		</form>
	</div>
</body>
</html>