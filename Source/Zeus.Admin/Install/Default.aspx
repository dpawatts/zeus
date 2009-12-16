<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Install.Default" %>
<%@ Register TagPrefix="zeus" Assembly="Zeus" Namespace="Zeus.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title>Install Zeus</title>
	<style>
		form{width:900px;margin:10px auto;}
		a{color:#00e;}
		li{margin-bottom:10px}
		ul { list-style-type:disc; margin:0 0 0 10px; padding:0; }
		form{padding:20px}
		.warning{color:#f00;}
		.ok{color:#0d0;}
		textarea{width:80%;height:120px}
		h1, h2, h3 { margin-top: 10px; margin-bottom: 10px;}
		h1 { color: #BCBEC0; font-size: 22px; margin-top: 0; }
		p { margin-bottom: 10px; }
		body { line-height: normal; }
	</style>
</head>
<body>
	<form runat="server">
		<asp:Literal ID="ltStartupError" runat="server" />

		<zeus:TabControl runat="server" ID="tbcInstallation">
			<zeus:TabItem runat="server" ID="tbiWelcome" ToolTip="1. Welcome">
				<h1>Install Zeus CMS</h1>
				<p class='<%# Status.IsInstalled ? "ok" : "warning" %>'>
				<b> Advice: </b> <%# GetStatusText() %></p>
				<p>To install Zeus you need to create a database, update the connection string in web.config, create the tables needed by Zeus, add the root node to the database and make sure the root node's id is configured in web.config.</p>
				<p>The following tabs will help you in these. Just continue to tab 2.</p>
				<h3>System status</h3>
				<p><%# Status.ToStatusString() %></p>
			</zeus:TabItem>
			
			<zeus:TabItem runat="server" ID="tbiDatabaseConnection" ToolTip="2. Database connection">
				<h1>Check database connection</h1>
				<asp:Literal runat="server" Visible='<%# Status.IsConnected %>'>
				<p class="ok"><b>Advice: </b>Since your database seems connected you may skip this step.</p>
				</asp:Literal>
				<p>First make sure you have a database available and configure the connection string in web.config.</p>
				<p>You can also create a database now by specifying the database server and database name.<br />
				Server: <asp:TextBox runat="server" ID="txtDatabaseServer" Text=".\SQLEXPRESS" />
				Database: <asp:TextBox runat="server" ID="txtDatabaseName" />
				<asp:Button runat="server" ID="btnCreateDatabase" Text="Create database and update web.config" OnClick="btnCreateDatabase_Click" /></p>
				<p><asp:Literal runat="server" ID="ltlCreateDatabase" /></p>
				<p>Once you're done you can <asp:Button ID="btnTest" runat="server" OnClick="btnTest_Click" Text="test the connection" CausesValidation="false" /></p>
				<p><asp:Label ID="lblStatus" runat="server" /></p>
			</zeus:TabItem>
			
			<zeus:TabItem runat="server" ID="tbiDatabaseTables" ToolTip="3. Database tables">
				<h1>Create database tables</h1>
				<asp:Literal runat="server" Visible='<%# Status.HasSchema %>'>
				<p class="ok"><b>Advice: </b>The database tables are okay. You can move to the next tab (if you create them again you will delete any existing content).</p>
				</asp:Literal>
				<asp:Literal runat="server" Visible='<%# !Status.IsConnected %>'>
				<p class="warning"><b>Advice: </b>Go back and check database connection.</p>
				</asp:Literal>
				<p>
					Assuming the database connection is okay (step 2) you can 
					<asp:Button ID="btnInstall" runat="server" OnClick="btnInstall_Click" Text="create tables" OnClientClick="return confirm('Creating database tables will destory any existing data. Are you sure?');" ToolTip="Click this button to install database" CausesValidation="false" />
					for the connection type <%= Status.ConnectionType %>.
				</p>
				<p><asp:Label CssClass="ok" runat="server" ID="lblInstall" /></p>
			</zeus:TabItem>

			<zeus:TabItem runat="server" ID="tbiRootNode" ToolTip="4. Root node">
				<h1>Insert root node (required)</h1>
				
				<asp:Literal runat="server" Visible='<%# Status.IsInstalled %>'>
				<p class="ok"><b>Advice: </b>The root and start nodes are configured and present in the database. If you create more they will become detached nodes cluttering the database unless you specify them in web.config (which makes the existing nodes detached instead).</p>
				</asp:Literal>
				
				<asp:Literal runat="server" Visible='<%# !Status.HasSchema %>'>
				<p class="warning"><b>Advice: </b>Go back and check database connection and tables.</p>
				</asp:Literal>
				
				<p>Zeus needs a root node and a start page in order to function correctly.
				These two "nodes" may be the same page for simple sites, e.g. if you don't forsee using multiple domains.</p>

				<ul>
					<li>
						Either, Select one 
						<asp:DropDownList ID="ddlRoot" runat="server" EnableViewState="false" />
						and one 
						<asp:DropDownList ID="ddlStartPage" runat="server" EnableViewState="false" />
						to 
						<asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Text="insert" ToolTip="Insert different root and start nodes" CausesValidation="false" />
						as <b>two different</b> nodes.
				    <asp:CustomValidator ID="cvRootAndStart" runat="server" ErrorMessage="Root and start type required" Display="Dynamic" />
					</li>
					<!--li>
						Or, use the <b>one node</b> for both
						<asp:DropDownList ID="ddlRootAndStart" runat="server" EnableViewState="false" />
						to
						<asp:Button ID="btnInsertRootOnly" runat="server" OnClick="btnInsertRootOnly_Click" Text="insert" ToolTip="Insert one node as root and start" CausesValidation="false" />.
						<asp:CustomValidator ID="cvRoot" runat="server" ErrorMessage="Root type required" Display="Dynamic" />
					</li-->
					<li>
						Or, select an export file 
						<asp:FileUpload ID="fileUpload" runat="server" />
						(*.zeus.xml or *.zeus.xml.gz) to
						<asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="upload and insert" ToolTip="Upload root node." CausesValidation="false" />
						<asp:RequiredFieldValidator ID="rfvUpload" ControlToValidate="fileUpload" runat="server" Text="Select import file" Display="Dynamic" EnableClientScript="false" />
					</li>
				</ul>
				
				<p><asp:Literal ID="ltRootNode" runat="server" /></p>
				
				<asp:PlaceHolder ID="phSame" runat="server" Visible="false">
					<h4>Example web.config with same root as start page</h4>
					<p>
						<textarea rows="4">
...
	<zeus>
		<host rootItemID="<%# RootId %>">
			<sites>
				<site id="DefaultSite" description="Default Site" startPageID="<%# StartId %>">
					<siteHosts>
						<add name="*" />
					</siteHosts>
				</site>
			</sites>
		</host>
		...
	</zeus>
...</textarea>
						<asp:Button runat="server" OnClick="btnUpdateWebConfig_Click" Text="Update web.config" CausesValidation="false" />
					</p>
				</asp:PlaceHolder>
				<asp:PlaceHolder ID="phDiffer" runat="server" Visible="false">
					<h4>Example web.config with different root as start pages</h4>
					<p>
						<textarea rows="4">
...
	<zeus>
		<host rootItemID="<%# RootId %>">
			<sites>
				<site id="DefaultSite" description="Default Site" startPageID="<%# StartId %>">
					<siteHosts>
						<add name="*" />
					</siteHosts>
				</site>
			</sites>
		</host>
		...
	</zeus>
...</textarea>
						<asp:Button runat="server" OnClick="btnUpdateWebConfig_Click" Text="Update web.config" />
					</p>
				</asp:PlaceHolder>
				<p><asp:Label runat="server" ID="lblWebConfigUpdated" /></p>
			</zeus:TabItem>
			
			<zeus:TabItem runat="server" ID="tbiUsers" ToolTip="5. Users">
				<h1>Create administrator login</h1>
				<asp:Literal runat="server" Visible='<%# !Status.HasSchema %>'>
				<p class="warning"><b>Advice: </b>Go back and check database connection and tables and root node.</p>
				</asp:Literal>
				<asp:Literal runat="server" Visible='<%# Status.HasUsers %>'>
				<p class="ok"><b>Advice: </b>An administrator login is already present in the database. To create more users, please login to the administration site and create users there.</p>
				</asp:Literal>
				<p>Please enter the details for an administrator login. Once you have finished this installation process,
				you will need to login to the admin site as this user.</p>
				<p>Username: <asp:TextBox runat="server" ID="txtAdministratorUsername" />
				Password: <asp:TextBox runat="server" ID="txtAdministratorPassword" />
				<asp:Button runat="server" ID="btnCreateAdministrator" Text="Create" OnClick="btnCreateAdministrator_Click" /></p>
				<p><asp:Literal runat="server" ID="ltlAdminLogin" /></p>
			</zeus:TabItem>

			<zeus:TabItem runat="server" ID="tbiFinishingTouches" ToolTip="6. Finishing touches">
				<h1>Almost done!</h1>
				<p><b>IMPORTANT!</b> Now that you've finished installing Zeus, you need to turn off the installation mode in web.config.
				The easiest method is to <asp:Button runat="server" OnClick="btnTurnOffInstallationMode_Click" Text="click here" CausesValidation="false" /> to do that automatically.</p>
				<p><asp:Literal runat="server" ID="ltlInstallationMode" /></p>
				<p>It's advisable to <asp:Button runat="server" OnClick="btnRestart_Click" Text="restart" CausesValidation="false" />
				before you continue.</p>
			</zeus:TabItem>
			
			<asp:Label EnableViewState="false" ID="errorLabel" runat="server" CssClass="errorLabel" />
		</zeus:TabControl>
	</form>
</body>
</html>