<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Popup.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.FileManager.Default" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<link rel="stylesheet" href="../assets/css/shared.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	<link rel="stylesheet" href="../assets/css/tree.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	<script type="text/javascript" src="../assets/js/jquery.js"></script>
	<script type="text/javascript" src="../assets/js/plugins/jquery.simpleTree.js"></script>
	<script type="text/javascript">
		$(document).ready(function() {
			simpleTreeCollection = $('.simpleTree').simpleTree({
				autoclose: false,
				afterClick: function(node) {
					top.preview.location.href = $("a:first", node).attr("href");
					//alert("text-"+$('span:first',node).text());
				},
				moving: function(destination, source) {
					return confirm("Are you sure you wish to move this item?");
				},
				afterMove: function(destination, source, pos) {
					var sourceID = $("span:first", source).attr('data-id');
					var sourceType = $("span:first", source).attr('data-type');
					var destinationID = $("span:first", destination).attr('data-id');
					var destinationType = $("span:first", destination).attr('data-type');
					top.preview.location.href = "/admin/move.aspx?sourcetype=" + sourceType + "&sourceid=" + sourceID + "&destinationtype=" + destinationType + "&destinationid=" + destinationID + "&pos=" + pos;
				},
				animate: true,
				docToFolderConvert: true
			});
		});
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<asp:PlaceHolder runat="server" ID="plcFileTree" />
	
	
</asp:Content>