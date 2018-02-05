<%@ Page Title="Dynamic Content" Language="C#" MasterPageFile="~/Admin/Popup.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.DynamicContent.Default" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
	<style type="text/css">
		body { margin: 5px; }
	</style>
  
  <script type="text/javascript">
  	var DynamicContentDialog = {
  		init: function(ed) {
  			//var action, elm, f = document.forms[0];

  			this.editor = ed;
  			/*elm = ed.dom.getParent(ed.selection.getNode(), 'A,IMG');
  			v = ed.dom.getAttrib(elm, 'name');

  			if (v) {
  			this.action = 'update';
  			f.anchorName.value = v;
  			}

  			f.insert.value = ed.getLang(elm ? 'update' : 'insert');*/
  		},

  		update: function() {
  			var ed = this.editor;

  			tinyMCEPopup.restoreSelection();

  			//if (this.action != 'update')
  			//	ed.selection.collapse(1);

  			// Webkit acts weird if empty inline element is inserted so we need to use a image instead
 				ed.execCommand('mceInsertContent', 0, '<asp:Literal runat="server" ID="ltlRenderedContentElement" />');

  			tinyMCEPopup.close();
  		}
  	};

  	tinyMCEPopup.onInit.add(DynamicContentDialog.init, DynamicContentDialog);
	</script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<asp:PlaceHolder runat="server" ID="plcStep1">
		<fieldset>
			<legend>Plugin</legend>
			
			<div class="editDetail">
				<asp:Label runat="server" AssociatedControlID="ddlPlugin" CssClass="editorLabel">Select plugin</asp:Label>
				<asp:DropDownList runat="server" ID="ddlPlugin" DataValueField="Name" DataTextField="Name" OnSelectedIndexChanged="ddlPlugin_SelectedIndexChanged" AutoPostBack="true" />
			</div>
			<div class="editDetail">
				<asp:Label runat="server" AssociatedControlID="lblDescription" CssClass="editorLabel">Description</asp:Label>
				<asp:Label runat="server" ID="lblDescription" />
			</div>
		</fieldset>
		
		<fieldset>
			<legend>Settings</legend>
			
			<zeus:ItemEditView runat="server" ID="zeusItemEditor" />
		</fieldset>
		
		<asp:Button runat="server" ID="btnOK" Text="OK" OnClick="btnOK_Click" />
	</asp:PlaceHolder>
	<asp:PlaceHolder runat="server" ID="plcStep2" Visible="false">
		<script type="text/javascript">
			tinyMCEPopup.onInit.add(DynamicContentDialog.update, DynamicContentDialog);
		</script>
	</asp:PlaceHolder>
</asp:Content>
