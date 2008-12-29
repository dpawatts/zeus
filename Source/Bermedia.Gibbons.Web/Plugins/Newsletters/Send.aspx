<%@ Page Language="C#" MasterPageFile="~/Admin/PreviewFrame.Master" AutoEventWireup="true" CodeBehind="Send.aspx.cs" Inherits="Bermedia.Gibbons.Web.Plugins.Newsletters.Send" %>
<%@ Import Namespace="Isis" %>
<%@ Import Namespace="Bermedia.Gibbons.Web.Items" %>
<asp:Content runat="server" ContentPlaceHolderID="Head">
	<link rel="stylesheet" href="/admin/assets/css/shared.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
	<link rel="stylesheet" href="/admin/assets/css/view.css" type="text/css" media="screen" title="Default Style" charset="utf-8"/>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<asp:Button runat="server" ID="btnSend" CssClass="save" Text="Start" OnClick="btnSend_Click" Visible='<%$ Code:SelectedNewsletter.Status == NewsletterStatus.NotStarted %>' /> 
	<asp:Button runat="server" ID="btnResume" CssClass="save" Text="Resume" OnClick="btnResume_Click" Visible='<%$ Code:SelectedNewsletter.Status == NewsletterStatus.Failed %>' /> 
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<asp:PlaceHolder runat="server" Visible='<%$ Code:SelectedNewsletter.Status == NewsletterStatus.NotStarted %>'>
		<h2>Begin Mailout</h2>
		<p><b>Once you have started the mailout, it will not be possible to stop it.<br />
		Please ensure all mailout settings are correct before clicking Start.</b></p>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder runat="server" Visible='<%$ Code:SelectedNewsletter.Status == NewsletterStatus.Failed %>'>
		<h2>Resume Mailout</h2>
		<p><b>You can click Resume to restart a mailout which has failed.
		Emails will not be sent to people who have already received one.</b></p>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder runat="server" Visible='<%$ Code:SelectedNewsletter.Status == NewsletterStatus.RunningReport || SelectedNewsletter.Status == NewsletterStatus.InProgress %>'>
		<p>This page will automatically refresh every 10 seconds, until the mailout is complete.
		You can also click the Refresh link to refresh the page manually.</p>
		<br />
		<p><a href="send.aspx?selected=<%= Server.UrlEncode(this.SelectedNewsletter.Path) %>&started=true">Refresh</a></p>
	</asp:PlaceHolder>
	
	<br />
	<h3>Status</h3>
	<p><%= this.SelectedNewsletter.Status.GetDescription() %><br />
	<asp:PlaceHolder runat="server" Visible='<%$ Code:SelectedNewsletter.Status == NewsletterStatus.InProgress || SelectedNewsletter.Status == NewsletterStatus.Failed || SelectedNewsletter.Status == NewsletterStatus.Successful %>'>
		<%= SelectedNewsletter.CurrentMessage %> out of <%= SelectedNewsletter.TotalMessages %>
	</asp:PlaceHolder>
	<br /><br />
	<span class="error"><%= SelectedNewsletter.ErrorMessage%></span></p>

	<h3>Mailout Log</h3>
	<isis:TypedListView runat="server" ID="lsvMailoutLog" DataSourceID="cdsNewsletterLog" DataItemTypeName="Bermedia.Gibbons.Web.Items.NewsletterLogEntry, Bermedia.Gibbons.Web">
		<LayoutTemplate>
			<div class="pageNo">
				<asp:DataPager runat="server" PageSize="200" QueryStringField="page">
					<Fields>
						<isis:GooglePagerField NextPageImageUrl="~/Admin/Assets/Images/View/button_arrow_right.gif"
							PreviousPageImageUrl="~/Admin/Assets/Images/View/button_arrow_left.gif" />
					</Fields>
				</asp:DataPager>
			</div>
			<table class="tb" border="0">
				<tr class="titles">
					<th>ID</th>
					<th>Email Address</th>
					<th>Status</th>
					<th>Error Message</th>
				</tr>
				<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
			</table>
			<div class="pageNo">
				<asp:DataPager runat="server" PageSize="200" QueryStringField="page">
					<Fields>
						<isis:GooglePagerField NextPageImageUrl="~/Admin/Assets/Images/View/button_arrow_right.gif"
							PreviousPageImageUrl="~/Admin/Assets/Images/View/button_arrow_left.gif" />
					</Fields>
				</asp:DataPager>
			</div>
		</LayoutTemplate>
		<ItemTemplate>
			<tr>
				<td><%# Container.DataItem.ID %></td>
				<td><%# ((Bermedia.Gibbons.Web.Items.NewsletterSubscription) Container.DataItem.Parent).EmailAddress %></td>
				<td><%# Container.DataItem.Status.GetDescription() %></td>
				<td><%# Container.DataItem.ErrorMessage %></td>
			</tr>
		</ItemTemplate>
		<EmptyDataTemplate>
			<p>No entries.</p>
		</EmptyDataTemplate>
	</isis:TypedListView>

	<zeus:ContentDataSource runat="server" ID="cdsNewsletterLog" Axis="Descendant" Query="RootItem"
		Where="Newsletter.ID == @NewsletterID" OfType="Bermedia.Gibbons.Web.Items.NewsletterLogEntry" />
</asp:Content>
