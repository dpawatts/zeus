<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeus.Admin.Navigation.Tree" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title>Navigation</title>
	
	<link rel="stylesheet" href="../assets/css/tree.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	
	<script type="text/javascript" src="../assets/js/jquery.js"></script>
	<script type="text/javascript" src="../assets/js/plugins/jquery.simpleTree.js"></script>
	<script type="text/javascript" src="../assets/js/plugins/jquery.easing.js"></script>
	<script type="text/javascript" src="../assets/js/plugins/jquery.easing.compatibility.js"></script>
	<script type="text/javascript" src="../assets/js/plugins/jquery.dimensions.js"></script>
	<script type="text/javascript" src="../assets/js/plugins/jquery.contextMenu.js"></script>
	
	<script type="text/javascript">
		var simpleTreeCollection;

		function initContextMenu() {
			$("ul.simpleTree li span").contextMenu({
				menu: 'myMenu'
			},
			function(action, el, pos) {
				switch (action) {
					case "new":
						top.preview.location.href = "/admin/add.aspx?parenttype=" + $(el).attr("data-type") + "&parentid=" + $(el).attr("data-id");
						break;
					case "edit":
						top.preview.location.href = "/admin/" + $(el).attr("data-adminurl") + "/edit.aspx?id=" + $(el).attr("data-id");
						break;
					case "delete":
						if (confirm("Are you sure you wish to delete this item?"))
							top.preview.location.href = "/admin/delete.aspx?type=" + $(el).attr("data-type") + "&id=" + $(el).attr("data-id");
						break;
				}
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
				afterMove: function(destination, source, pos) {
					if (confirm("Are you sure you wish to move this item?")) {
						var sourceID = $("span:first", source).attr('data-id');
						var sourceType = $("span:first", source).attr('data-type');
						var destinationID = $("span:first", destination).attr('data-id');
						var destinationType = $("span:first", destination).attr('data-type');
						top.preview.location.href = "/admin/move.aspx?sourcetype=" + sourceType + "&sourceid=" + sourceID + "&destinationtype=" + destinationType + "&destinationid=" + destinationID + "&pos=" + pos;
					}
				},
				afterAjax: function() {
					initContextMenu();
				},
				animate: true
				//,docToFolderConvert:true
			});

			initContextMenu();
		});
	</script>
</head>
<body>
	<div id="container">
		<asp:PlaceHolder runat="server" ID="plcTree" />
	</div>
	
	<ul id="myMenu" class="contextMenu">
    <li class="new">
        <a href="#new">New</a>
    </li>
    <li class="edit">
        <a href="#edit">Edit</a>
    </li>
    <li class="delete">
        <a href="#delete">Delete</a>
    </li>
	</ul>
	
	<form runat="server"></form>
</body>
</html>