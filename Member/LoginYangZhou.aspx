<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginYangZhou.aspx.cs"
    Inherits="NetRadio.Web.Member.__LoginYangZhou" %>

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
        .aa
        {
            background-image: url(../images/indexBG.jpg);
            background-position: center center;
            background-repeat: no-repeat;
            color: White;
            font-weight: bold;
        }
        .bb
        {
            margin-top: 40px;
        }
    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <div>
        <table class="aa" width="1261" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="624">
                    <table class="bb" width="494" align="center" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="80" colspan="3" align="center" valign="top" style="font-size: 30px;
                                font-family: 微软雅黑;  font-weight:normal;  color: White;">
                                <%= NetRadio.Business.BusAppInfo.Name%>
                            </td>
                        </tr>
                        <tr>
                            <td width="79" height="50" align="center" valign="middle">
                                用户名&nbsp;&nbsp;
                            </td>
                            <td width="278" valign="middle">
                                <asp:TextBox ID="userName" Width="200" MaxLength="24" Height="20" Style="border: 0"
                                    runat="server" TabIndex="1" />
                            </td>
                            <td width="137" rowspan="2" align="left" valign="middle">
                                <asp:ImageButton ID="submit" ImageUrl="../Images/indexlog.jpg" OnClick="submit_Click"
                                    AlternateText="登陆" ForeColor="Black" runat="server" TabIndex="3" align="absmiddle" />
                            </td>
                        </tr>
                        <tr>
                            <td height="50" align="center" valign="middle">
                                密 码&nbsp;&nbsp;
                            </td>
                            <td valign="middle">
                                <asp:TextBox TextMode="Password" ID="password" Width="200" Height="20" MaxLength="16"
                                    Style="border: 0" runat="server" TabIndex="2" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" border="0" align="center">
            <tr>
                <td valign="middle" align="center" height="100" style="font-weight: lighter;" colspan="3">
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
    </div>
    </form>
</body>
</html>
