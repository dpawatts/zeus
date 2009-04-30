<%@ Page Language="C#" MasterPageFile="~/Admin/PreviewFrame.Master" AutoEventWireup="true" CodeBehind="ViewChildren.aspx.cs" Inherits="Zeus.Admin.ViewChildren" %>
<asp:Content ContentPlaceHolderID="Head" runat="server">
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
			tb_init('#grid a.thickbox');

			delButtons();
		}
	}
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="Toolbar" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<h2><asp:Literal runat="server" Text="<%$ CurrentPage:Title %>" /></h2>
	
	<p><span class="add"><a class="thickbox" href="ViewDetail.aspx?selected=<asp:Literal runat="server" Text="<%$ Code:HttpUtility.UrlEncode(SelectedItem.Path) %>" />&discriminator=<asp:Literal runat="server" ID="ltlDiscriminator" />&TB_iframe=true&height=400&width=700">Add</a></span></p>
	<br />
	
	<div id="grid">
		<asp:UpdatePanel runat="server" ID="updUpdatePanel">
			<ContentTemplate>
				<zeus:ItemGridView runat="server" ID="zeusItemGridView" DataSourceID="cdsChildren" />
				<zeus:ContentDataSource runat="server" ID="cdsChildren" OrderBy="Title" />
				<asp:Button runat="server" ID="btnRefreshGrid" style="display:none" OnClick="btnRefreshGrid_Click" />
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
</asp:Content>