<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Default" %>
<%@ Register TagPrefix="isis" Namespace="Isis.Web.UI.WebControls" Assembly="Isis.FrameworkBlocks.WebSecurity" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

	<title><asp:Literal runat="server" ID="ltlAdminName1" /> Administration</title>
	
	<script type="text/javascript">
		$(document).ready(function() {
			window.zeus = new frameManager();
		});

		/*function reloadContentPanel(title, url) {
			Ext.getCmp("pnlContent").setTitle(title);
			document.getElementById("pnlContent_IFrame").src = url;
		}*/
	</script>
</head>
<body>
	<ext:ScriptManager runat="server" Theme="Gray" />
	
	<noscript><div id="js">
		<p><span class="bold">NOTE: </span>Javascript is turned off. You must have javascript turned on to use this interface. For instructions, please contact us by clicking <a href="http://www.sitdap.com">here</a></p>
	</div></noscript>
	
	<form runat="server">
		<ext:ViewPort runat="server">
			<Body>
				<ext:BorderLayout runat="server">
					<North Margins-Bottom="5">
						<ext:Panel runat="server">
							<Body>
								<div id="header">
									<img runat="server" id="imgLogo" border="0" alt="Sound In Theory"/>
									<p id="title">administration site for <span><asp:Literal runat="server" ID="ltlAdminName2" /></span></p>
									<div id="headerRight">
										<isis:LoginStatus runat="server" ID="logOut" />
										<p id="loggedAs">You are logged in as <isis:LoginName runat="server" /></p>
									</div>
								</div>

								<div id="toolbar">
									<asp:PlaceHolder runat="server" ID="plcToolbar" />
								</div>
							</Body>
						</ext:Panel>
					</North>
					<West Split="true" MinWidth="175" MaxWidth="400">
						<ext:TreePanel runat="server" ID="stpNavigation" Width="200" Icon="SitemapColor" Title="Site" AutoScroll="true" PathSeparator="|" EnableDD="true">
							<TopBar>
								<ext:Toolbar runat="server">
									<Items>
										<ext:ToolbarFill runat="server" />
										<ext:ToolbarButton runat="server" Icon="Reload">
											<ToolTips>
												<ext:ToolTip Html="Refresh" />
											</ToolTips>
											<Listeners>
												<Click Handler="#{stpNavigation}.getLoader().load(#{stpNavigation}.getRootNode());" />
											</Listeners>
										</ext:ToolbarButton>
										<ext:ToolbarButton runat="server" IconCls="icon-expand-all">
											<ToolTips>
												<ext:ToolTip Html="Expand All" />
											</ToolTips>
											<Listeners>
												<Click Handler="#{stpNavigation}.expandAll();" />
											</Listeners>
										</ext:ToolbarButton>
										<ext:ToolbarButton runat="server" IconCls="icon-collapse-all">
											<ToolTips>
												<ext:ToolTip Html="Collapse All" />
											</ToolTips>
											<Listeners>
												<Click Handler="#{stpNavigation}.collapseAll();" />
											</Listeners>
										</ext:ToolbarButton>
									</Items>
								</ext:Toolbar>
							</TopBar>
							<Listeners>
								<ContextMenu Fn="zeus.showContextMenu" />
							</Listeners>
							<AjaxEvents>
								<MoveNode Url="/admin/default.aspx" OnEvent="OnMoveNode" Before="#{stbStatusBar}.showBusy();">
									<ExtraParams>
										<ext:Parameter Name="source" Value="node.id" Mode="Raw" />
										<ext:Parameter Name="destination" Value="newParent.id" Mode="Raw" />
										<ext:Parameter Name="pos" Value="index" Mode="Raw" />
									</ExtraParams>
								</MoveNode>
							</AjaxEvents>
							<Loader>
								<ext:TreeLoader DataUrl="/admin/Navigation/SiteTreeLoader.ashx" PreloadChildren="true" />
							</Loader>
						</ext:TreePanel>
					</West>
					<Center>
						<ext:Panel runat="server" ID="pnlContent" Title="Preview" Icon="Page">
							<AutoLoad Url="/" Mode="IFrame" />
						</ext:Panel>
					</Center>
					<South>
						<ext:StatusBar runat="server" ID="stbStatusBar" AutoClear="1500" />
					</South>
				</ext:BorderLayout>
			</Body>
		</ext:ViewPort>
	</form>
</body>
</html>