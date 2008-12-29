<%@ Page Language="C#" MasterPageFile="~/Admin/PreviewFrame.Master" CodeBehind="SendTest.aspx.cs" Inherits="Bermedia.Gibbons.Web.Plugins.Newsletters.SendTest" %>
<%@ Register TagPrefix="uc" TagName="Entity" Src="Entity.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
	<div id="editToolbar">
		<asp:Button runat="server" ID="btnSend" CssClass="save" Text="Send" OnClick="btnSend_Click" /> 
	</div>
	
	<div style="padding: 10px;">
		<h2 id="itemTitle">Send Test Email</h2>
		<asp:PlaceHolder runat="server" ID="plcForm">
			<div id="edit">
				<div id="validation">
					<asp:CustomValidator runat="server" ID="csvUnknownError" ValidationGroup="UnknownErrors" />
					<asp:ValidationSummary runat="server" CssClass="validationSummary" />
				</div>
				
				<asp:Label runat="server" AssociatedControlID="txtEmailAddress">Email Address</asp:Label>
				<asp:TextBox runat="server" ID="txtEmailAddress" MaxLength="100" CssClass="required wide" />
			</div>
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="plcConfirmation" Visible="false">
			<p class="aloneText">The test email has now been sent.</p>
		</asp:PlaceHolder>
	</div>
</asp:Content>
