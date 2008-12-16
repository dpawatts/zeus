<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Install.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Migrations run.
    
    <p>Root Item: <asp:DropDownList runat="server" ID="ddlRootItem" /></p>
    
    <p><asp:Button runat="server" ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" /></p>
    
    <p><asp:Button runat="server" ID="btnInstallDynamicImageCaching" Text="Install DynamicImage Caching" OnClick="btnInstallDynamicImageCaching_Click" /></p>
    </div>
    </form>
</body>
</html>
