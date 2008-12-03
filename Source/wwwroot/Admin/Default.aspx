<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Edit.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Zeus Administration</title>
	
	<link rel="stylesheet" href="assets/css/reset.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	<link rel="stylesheet" href="assets/css/default.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	
	<script type="text/javascript" src="assets/js/jquery.js"></script>
	<script type="text/javascript" src="assets/js/plugins/jquery.splitter.js"></script>
	<script type="text/javascript" src="assets/js/zeus.js"></script>
	<script type="text/javascript">
	
	$(document).ready(function() {
		window.zeus = new frameManager();
		zeus.initFrames();
	});
	</script>
</head>
<body>
	<noscript><div id="js">
		<p><span class="bold">NOTE: </span>Javascript is turned off. You must have javascript turned on to use this interface. For instructions, please contact us by clicking <a href="http://www.sitdap.com">here</a></p>
	</div></noscript>
	
	<form runat="server">
		<div id="header" class="clearfix">
			<img src="assets/images/theme/logo.gif" border="0" alt="Sound In Theory"/>
			<p id="title"><span>zeus</span> administration site</p>
			<div id="headerRight">
				<asp:LoginStatus runat="server" ID="logOut" />
				<p id="loggedAs">You are logged in as <asp:LoginName runat="server" /></p>
			</div>
		</div>
		
		<div id="toolbar">
			<a href="/admin/filemanagement/default.aspx" target="preview">Files</a>
			<div class="separator">&nbsp;</div>
		</div>
		
		<div id="splitter">
			<div id="LeftPane">
				<iframe id="navigation" src="navigation/tree.aspx" frameborder="0" name="navigation" width="25%" height="500"></iframe>
			</div>
			<div id="RightPane">
				<iframe id="preview" src="/default.aspx" frameborder="0" name="preview" width="75%" height="500"></iframe>
			</div>
		</div>
	</form>
</body>
</html>