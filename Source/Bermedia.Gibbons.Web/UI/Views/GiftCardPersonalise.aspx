<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="GiftCardPersonalise.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.GiftCardPersonalise" %>
<%@ Register TagPrefix="gibbons" TagName="GiftCardNavigation" Src="../UserControls/GiftCardNavigation.ascx" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content runat="server" ContentPlaceHolderID="cphHead">
	<script type="text/javascript" src="/assets/js/swfobject.js"></script>
	<script type="text/javascript" src="/assets/js/swfaddress.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:GiftCardNavigation runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Personalise Your Gift Card</h1>

	<asp:MultiView runat="server" ID="mlvPersonalise" ActiveViewIndex="0">
		<asp:View runat="server">
			<p><strong>Step 1:</strong> select dollar value of your gift card</p>
	
			<table cellpadding="3" border="0">
				<tr>
					<td valign="center" style="white-space: nowrap" align="right">
						Choose gift card amount:
						<select onchange="$('#<%= txtAmount.ClientID %>').val($(this).val());">
							<option value="0" selected>Select</option>
							<option value="25">$25</option>
							<option value="50">$50</option>
							<option value="75">$75</option>
							<option value="100">$100</option>
							<option value="150">$150</option>
							<option value="200">$200</option>
							<option value="250">$250</option>
							<option value="300">$300</option>
							<option value="400">$400</option>
							<option value="500">$500</option>
						</select>
						&nbsp;&nbsp;
						or enter amount: $
						<asp:TextBox runat="server" ID="txtAmount" MaxLength="4" Width="30" />.00
						<asp:CustomValidator runat="server" ID="csvAmount" ControlToValidate="txtAmount" ValidateEmptyText="true" ErrorMessage="Please enter an amount" Text="*" OnServerValidate="csvAmount_ServerValidate" />
						&nbsp;&nbsp;
						Quantity: <isis:DropDownList runat="server" ID="ddlQuantity" Width="60px" DataSource='<%# Enumerable.Range(1, 12) %>' RequiresDataBinding="true" />
					</td>
				</tr>
			</table>
			
			<br />
			
			<table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
				<td valign="top" align="left" style="padding-right: 10px;">
					<img src="/assets/images/giftcards/cardBlank.gif" alt="" width="300" height="189">
				</td>
				<td valign="top">
					<strong>Create a Gift Card that reflects you!</strong><br />
					Personalizing your Gift Card is easy! Upload a digital image or add a message!<br />
					<br />
					<asp:ValidationSummary runat="server" ID="vlsSummary" />
					<br />
					<sitdap:DynamicImageButton runat="server" ID="btnContinue" CommandName="NextView" TemplateName="Button" AlternateText="continue">
						<Layers>
							<sitdap:TextLayer Name="Text" Text="continue" />
						</Layers>
					</sitdap:DynamicImageButton>
				</td>
			</table>
		</asp:View>
		<asp:View runat="server">
    	<div id="flashWrap">
	    	<script type="text/javascript">
				// <![CDATA[
				
				var so = new SWFObject("/assets/swf/personaliseGiftCard.swf", "flashSite", "700", "400", "9", "#FFFFFF");
				so.addParam("scale", "noscale");
				so.addParam("menu", "false");
				so.addVariable("saveImage", "/GiftCardImages.axd?O=Save%26Am=<%= txtAmount.Text %>%26Qty=<%= ddlQuantity.SelectedValue %>");
				so.addVariable("uploadImage", "/GiftCardImages.axd?O=Upload");
				so.addVariable("xmlPath", "/assets/xml/giftCardConfig.xml");
				so.useExpressInstall('/assets/swf/expressinstall.swf');
				so.write("flashWrap");
				
				// ]]>
				</script>
			</div>
		</asp:View>
	</asp:MultiView>
	
	<br style="clear:both" />
</asp:Content>