<%@ Page Language="C#" MasterPageFile="../MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="FragranceBeautyDepartment.aspx.cs" Inherits="Bermedia.Gibbons.Web.UI.Views.FragranceBeautyDepartment" %>
<%@ Register TagPrefix="gibbons" TagName="DepartmentNavigation" Src="../UserControls/DepartmentNavigation.ascx" %>
<asp:Content ContentPlaceHolderID="cphSubNavigation" runat="server">
	<gibbons:DepartmentNavigation runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<h1>Shop Online</h1>
	
  <p>Shop for your favourite fragrances by brand, name or by scent. Order your selection online and 
  we&rsquo;ll gift wrap, package and have your order ready within 24 hours. You can collect your 
  package from our Church Street depot with a photo   ID, or we&rsquo;ll deliver it anywhere in Hamilton for free. 
  We&rsquo;ll also deliver island   &ndash; wide within 48 hours for a $17.50. Please note: For orders 
  placed during   public holidays or Sundays, allow 24 hour delivery from next working day.</p>

	<asp:Panel runat="server" DefaultButton="btnSearch">
		<table width="500" border="0" cellpadding="1" cellspacing="0">
			<tr>
				<td width="200"><asp:Label runat="server" AssociatedControlID="txtSearchText"><strong>Search for:</strong></asp:Label></td>
				<td>
					<asp:TextBox runat="server" ID="txtSearchText" MaxLength="50" />
					<asp:Button runat="server" ID="btnSearch" CssClass="mySubmit" Text="Go" OnClick="btnSearch_Click" />
				</td>
			</tr>
		</table>
	</asp:Panel>
	
	<br />
	OR
	<br />
	
	<asp:Panel runat="server" DefaultButton="btnChooseBrand">
		<table width="500" border="0" cellpadding="1" cellspacing="0">
			<tr>
				<td width="200"><asp:Label runat="server" AssociatedControlID="ddlBrands"><strong>Choose brand:</strong></asp:Label></td>
				<td>
					<asp:DropDownList runat="server" ID="ddlBrands" DataValueField="ID" DataTextField="Title" AppendDataBoundItems="true">
						<asp:ListItem Value="">Please select...</asp:ListItem>
					</asp:DropDownList>
					<asp:Button runat="server" ID="btnChooseBrand" CssClass="mySubmit" Text="Go" OnClick="btnChooseBrand_Click" />
				</td>
			</tr>
		</table>
	</asp:Panel>

	<br />
	OR
	<br />
	
	<asp:Panel runat="server" DefaultButton="btnGo2">
		<table width="500" border="0" cellpadding="1" cellspacing="0">
			<tr>
				<td width="200"><asp:Label runat="server" AssociatedControlID="ddlDepartments"><strong>Choose by men or womens:</strong></asp:Label></td>
				<td>
					<asp:DropDownList runat="server" ID="ddlDepartments" DataValueField="ID" DataTextField="Title" AppendDataBoundItems="true">
						<asp:ListItem Value="">Please select...</asp:ListItem>
					</asp:DropDownList>
					<asp:Button runat="server" ID="btnGo2" CssClass="mySubmit" Text="Go" OnClick="btnGo2_Click" />
				</td>
			</tr>
		</table>
	</asp:Panel>
  
	<br />
	OR
	<br />
	
	<asp:Panel runat="server" DefaultButton="btnChooseByScent">
		<table width="500" border="0" cellpadding="1" cellspacing="0">
			<tr>
				<td width="200"><asp:Label runat="server" AssociatedControlID="ddlScents"><strong>Choose by scent:</strong></asp:Label></td>
				<td>
					<asp:DropDownList runat="server" ID="ddlScents" DataValueField="ID" DataTextField="Title" AppendDataBoundItems="true">
						<asp:ListItem Value="">Please select...</asp:ListItem>
						<asp:ListItem Value="-1">All Scents</asp:ListItem>
					</asp:DropDownList>
					<asp:Button runat="server" ID="btnChooseByScent" CssClass="mySubmit" Text="Go" OnClick="btnChooseByScent_Click" />
				</td>
			</tr>
		</table>
	</asp:Panel>
</asp:Content>