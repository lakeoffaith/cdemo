<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="NetRadio.LocatingMonitor.test"
    Theme="Default" %>

<%@ Register Src="Controls/RoutePoliceAndCulprit.ascx" TagName="RoutePoliceAndCulprit"
    TagPrefix="uc1" %>
<%@ Register src="Controls/RoutePolice.ascx" tagname="RoutePolice" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="App_Script/func.js"></script>

    <script src="App_Script/Global.js"></script>

    <script src="App_Script/Common.js"></script>

    <script src="App_Script/Control.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div onclick="RoutePoliceAndCulprit.show()">
        测试1</div> <br /><div onclick="RoutePolice.show()">
        测试2</div><br />
    <div style="padding: 20px 0px 0px 20px;">
        <uc1:RoutePoliceAndCulprit ID="RoutePoliceAndCulprit1" runat="server" />
    </div>
    <p>
        &nbsp;<%--<uc2:RoutePolice ID="RoutePolice1" runat="server" />--%>
    </form>
    </body>
</html>
