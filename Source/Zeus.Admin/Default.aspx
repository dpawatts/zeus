<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Default" %>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Web.UI.WebControls" Assembly="Zeus" %>
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
		<ext:ViewPort runat="server" ID="extViewPort">
			<Body>
				<ext:BorderLayout runat="server" ID="extBorderLayout">
					<North Margins-Bottom="5">
						<ext:Panel runat="server">
							<Body>
								<div id="header">
									<img runat="server" id="imgLogo" border="0" alt="Sound In Theory"/>
									<p id="title">administration site for <span><asp:Literal runat="server" ID="ltlAdminName2" /></span></p>
									<div id="headerRight">
										<zeus:LoginStatus runat="server" ID="logOut" />
										<p id="loggedAs">You are logged in as <zeus:LoginName runat="server" /></p>
									</div>
								</div>

								<div id="toolbar">
									<asp:PlaceHolder runat="server" ID="plcToolbar" />
								</div>
							</Body>
							<BottomBar>
								<ext:Toolbar runat="server">
									<Items>
										<ext:Button runat="server" Text="Blogs" Icon="UserComment">
											<Menu>
												<ext:Menu runat="server">
													<Items>                    
														<ext:MenuItem runat="server" Text="Moderate Comments" Icon="Comments" />
													</Items>
												</ext:Menu>
											</Menu>
										</ext:Button>
										<ext:Button runat="server" Text="Forums" Icon="Comments">
											<Menu>
												<ext:Menu runat="server">
													<Items>                    
														<ext:MenuItem runat="server" Text="Moderate Posts" Icon="Comments" />
													</Items>
												</ext:Menu>
											</Menu>
										</ext:Button>
										<ext:Button runat="server" Text="E-Commerce" Icon="Money">
											<Menu>
												<ext:Menu runat="server">
													<Items>                    
														<ext:MenuItem runat="server" Text="Manage Orders" Icon="Comments" />
													</Items>
												</ext:Menu>
											</Menu>
										</ext:Button>
									</Items>
								</ext:Toolbar>
							</BottomBar>
						</ext:Panel>
					</North>
					<West Split="true" MinWidth="175" MaxWidth="400">
						
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