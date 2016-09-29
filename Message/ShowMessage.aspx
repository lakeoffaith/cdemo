<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="ShowMessage.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Message._ShowMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <style>
        #icon
        {
            font-size: 65px;
            color: #b3b3b3;
        }
        a:link
        {
            color: #2c78c5;
            text-decoration: none;
        }
        a:visited
        {
            color: #2c78c5;
            text-decoration: none;
        }
        a:hover
        {
            color: red;
            text-decoration: none;
        }
        a:active
        {
            color: #2c78c5;
            text-decoration: none;
        }
       <%-- #pagetemplate
        {
            border: solid 0px gray;
            padding: 0px;
            margin: 0px;
            background-color: White;
        }
        #pagecontent
        {
            border: solid 0px gray;
            padding: 0px;
            margin: 0px;
            background-color: White;
        }--%>
    </style>
    <br />
    <br />
    <table style="width: 460px;" border="0" cellpadding="0" cellspacing="0" align="center">
        <tr>
            <td style="background-image: url(../images/mesbg1.jpg);">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="background-image: url(../images/mesbg2.jpg); background-repeat: repeat-y;">
                <table style="width: 440px" align="center">
                    <tr>
                        <td id="tdCaption" runat="server" colspan="2" style="border-bottom: solid 1px #dddddd;">
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" width="50px" height="100">
                            <font id="icon" face="webdings">i</font>
                        </td>
                        <td valign="top" runat="server" id="tdLinks" style="padding-left: 20px;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="background-image: url(../images/mesbg3.jpg);">
                &nbsp;
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <br />
</asp:Content>
