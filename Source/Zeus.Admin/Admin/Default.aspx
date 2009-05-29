<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Default" %>
<%@ Register TagPrefix="isis" Namespace="Isis.Web.UI.WebControls" Assembly="Isis.FrameworkBlocks.WebSecurity" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

	<title><asp:Literal runat="server" ID="ltlAdminName1" /> Administration</title>
	
	<script type="text/javascript">
	$(document).ready(function() {
		window.zeus = new frameManager();
	});
	</script>
</head>
<body>
	<noscript><div id="js">
		<p><span class="bold">NOTE: </span>Javascript is turned off. You must have javascript turned on to use this interface. For instructions, please contact us by clicking <a href="http://www.sitdap.com">here</a></p>
	</div></noscript>
	
	<form runat="server">
	<div id="desktop">
		<div id="desktopHeader">
			<div id="desktopTitlebarWrapper">
				<div id="desktopTitlebar">
					<img runat="server" id="imgLogo" border="0" alt="Sound In Theory"/>
					<p id="title">administration site for <span><asp:Literal runat="server" ID="ltlAdminName2" /></span></p>
					<div id="headerRight">
						<isis:LoginStatus runat="server" ID="logOut" />
						<p id="loggedAs">You are logged in as <isis:LoginName runat="server" /></p>
					</div>
				</div>
			</div>
		
			<div id="desktopNavbar">	
				<asp:PlaceHolder runat="server" ID="plcToolbar" />
				<div class="toolbox divider">
					<div id="spinnerWrapper"><!--div id="spinner" style="display: none;"/--></div>
				</div>
			</div><!-- desktopNavbar end -->
		</div>
		
		<div id="dockWrapper">
			<div id="dock">
				<div id="dockPlacement"></div>
				<div id="dockAutoHide"></div>
				<div id="dockSort"><div id="dockClear" class="clear"></div></div>
			</div>
		</div>
		
		<div id="pageWrapper"></div>
		
		<div id="desktopFooterWrapper">
			<div id="desktopFooter">			
				© 2009 <a href="http://www.sitdap.com/" target="_blank">Sound in Theory Ltd</a></a>
			</div>
		</div>
	</div>
	</form>
</body>
</html>