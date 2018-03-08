<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Zeus.Admin.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title>Login</title>
	
	<script type="text/javascript">
		jQuery(document).ready(function() {
			if (window.self != window.top)
				top.location = self.location.href;
		});
	</script>
</head>

<body>

	<noscript><div id="js">
		<p><span class="bold">NOTE: </span>Javascript is turned off. You must have javascript turned on to use this interface. For instructions, please contact us by clicking <a href="http://www.sitdap.com">here</a></p>
	</div></noscript>
	
<div id="login">
	<p id="for">Administration Site For</p>
	<p id="webName"><asp:Literal runat="server" ID="ltlAdminName" /></p>
	<form runat="server" id="lgnLogin" defaultbutton="lgnLogin$loginButton">
		<asp:Label runat="server" AssociatedControlID="UserName">Username</asp:Label>
		<asp:TextBox runat="server" ID="UserName" />
		<asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" Text="*" /><br />
		
		<asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label>
		<asp:TextBox runat="server" ID="Password" TextMode="Password" />
		<asp:RequiredFieldValidator runat="server" ControlToValidate="Password" Text="*" /><br />
		
		<div id="tryAgain"><asp:Literal runat="server" ID="FailureText" /></div>
		
		<asp:Button runat="server" ID="loginButton" OnClick="loginButton_Click" />
	</form>
</div>

</body>
</html>