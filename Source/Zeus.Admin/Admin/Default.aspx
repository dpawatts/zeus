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

Ext.onReady(function() {

    // NOTE: This is an example showing simple state management. During development,
    // it is generally best to disable state management as dynamically-generated ids
    // can change across page loads, leading to unpredictable results.  The developer
    // should ensure that stable state ids are set for stateful components in real apps.
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());

    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [
                new Ext.BoxComponent({ // raw
                    region: 'north',
                    el: 'north',
                    height: 59
                }), {
                    region: 'west',
                    contentEl: 'west',
                    split: true,
                    width: 200,
                    minSize: 175,
                    maxSize: 400,
                    collapsible: true
                }, {
                    region: 'center',
                    contentEl: 'center'
                }
             ]
    });
});
	</script>
</head>
<body>
	<noscript><div id="js">
		<p><span class="bold">NOTE: </span>Javascript is turned off. You must have javascript turned on to use this interface. For instructions, please contact us by clicking <a href="http://www.sitdap.com">here</a></p>
	</div></noscript>
	
	<form runat="server">
	    <div id="north">
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
		</div>
		
		<div id="west" style="height:100%">
		    <iframe id="navigation" src="navigation/tree.aspx" frameborder="0" width="100%" height="100%"></iframe>
		</div>
		
		<div id="center" style="height:100%">
			<iframe id="content" src="/" frameborder="0" width="100%" height="100%"></iframe>
		</div>
	</form>
</body>
</html>