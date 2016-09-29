<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="NetRadio.Web.Member.__Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= NetRadio.Business.BusAppInfo.Name%>
        - 登录</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />

    <script type="text/javascript" src="/Jail/scripts/common.js"></script>

    <style type="text/css">
        body
        {
            background: #0464cb url(../images/bg2.jpg) center repeat-x;
        }
    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <div id="logbg">
        <div style="font-size: 30px; font-family: 微软雅黑; padding-left: 300px; padding-top: 20px;
            color: White;">
            <%= NetRadio.Business.BusAppInfo.Name%></div>
        <%--<img src="" height="200" width="1000"/>--%>
    </div>
    <br />
    <br />
    <table cellpadding="0" cellspacing="0" border="0" align="center" style="color:White; font-weight: bold;">
        <tr>
            <td valign="middle">
                &nbsp;&nbsp;用户名 &nbsp;&nbsp;<asp:TextBox ID="userName" Width="100" MaxLength="24"
                    Height="20" Style="border: 0" runat="server" TabIndex="1" />
            </td>
            <td valign="middle">
                &nbsp;&nbsp;密 码 &nbsp;&nbsp;<asp:TextBox TextMode="Password" ID="password" Width="100"
                    Height="20" MaxLength="16" Style="border: 0" runat="server" TabIndex="2" />
            </td>
            <td valign="middle">
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="submit" ImageUrl="../Images/lginput.gif"
                    OnClick="submit_Click" AlternateText="登陆" ForeColor="Black" runat="server" TabIndex="3"
                    align="absmiddle" />
            </td>
        </tr>
        <tr>
            <td valign="middle" align="center" height="100" style=" font-weight:lighter;" colspan="3">
                <% 
                    string icp = NetRadio.Business.BusSystemConfig.GetICP();
                    if (icp.Length > 0)
                    {
                        Response.Write(icp);
                    } 
                %>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
