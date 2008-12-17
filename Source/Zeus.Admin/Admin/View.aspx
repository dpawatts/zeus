<%@ Page Language="C#" MasterPageFile="~/Admin/PreviewFrame.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeus.Admin.View" %>
<asp:Content ContentPlaceHolderID="Head" runat="server">
	<link rel="stylesheet" href="assets/css/shared.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	<link rel="stylesheet" href="assets/css/view.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	<link rel="stylesheet" href="assets/css/thickbox.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	
	<script type="text/javascript" src="assets/js/jquery.js"></script>
	<script type="text/javascript" src="assets/js/plugins/thickbox.js"></script>
	<script type="text/javascript" src="assets/js/zeus.js"></script>
	
	<script type="text/javascript">
	function updated() {
		//  close the popup
		tb_remove();

		//  refresh the update panel so we can view the changes
		$('#<%= btnRefreshGrid.ClientID %>').click();
	}

	function pageLoad(sender, args) {
		if (args.get_isPartialLoad()) {
			//  reapply the thick box stuff
			tb_init('a.thickbox');
		}
	}
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<h2><asp:Literal runat="server" Text="<%$ CurrentPage:Title %>" /></h2>
	
	<p><span class="add"><a href="New.aspx?selected=<asp:Literal runat="server" Text="<%$ CurrentPage:Path %>" />">Add</a></span></p>
	<br />
		
	<asp:UpdatePanel runat="server">
		<ContentTemplate>
			<zeus:ItemGridView runat="server" ID="zeusItemGridView" />
			<asp:Button runat="server" ID="btnRefreshGrid" style="display:none" OnClick="btnRefreshGrid_Click" />
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>