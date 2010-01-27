<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Default" %>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Web.UI.WebControls" Assembly="Zeus" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

	<title><asp:Literal runat="server" ID="ltlAdminName1" /> Administration</title>
	
	<ext:ResourcePlaceHolder runat="server" Mode="Script" />
	<ext:ResourcePlaceHolder runat="server" Mode="Style" />
	
	<script type="text/javascript">
		Ext.onReady(function() {
			window.zeus = new frameManager();
		});

		/*function reloadContentPanel(title, url) {
			Ext.getCmp("pnlContent").setTitle(title);
			document.getElementById("pnlContent_IFrame").src = url;
		}*/
	</script>
</head>
<body>
	<ext:ResourceManager runat="server" Theme="Gray" />
	
	<noscript><div id="js">
		<p><span class="bold">NOTE: </span>Javascript is turned off. You must have javascript turned on to use this interface. For instructions, please contact us by clicking <a href="http://www.sitdap.com">here</a></p>
	</div></noscript>
	
	<form runat="server">
		<ext:Viewport runat="server" ID="extViewPort" Layout="Border">
			<Items>
				<ext:Panel runat="server" Region="North" Border="false">
					<Content>
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
					</Content>
					<BottomBar>
						<ext:Toolbar runat="server" ID="extTopToolbar" />
					</BottomBar>
				</ext:Panel>
				<ext:Panel runat="server" ID="pnlContent" Region="Center" Title="Preview" Icon="Page">
					<AutoLoad Mode="IFrame" />
				</ext:Panel>						
				<ext:StatusBar runat="server" ID="stbStatusBar" Region="South" MinHeight="25" Height="25" AutoClear="1500" />
			</Items>
		</ext:Viewport>
	</form>
</body>
</html>