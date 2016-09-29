<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="RoutePatrol.aspx.cs" Inherits="NetRadio.LocatingMonitor.Monitor.__RoutePatrol" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <input id="btnAddRoute" type="button" value="添加轨迹" onclick="return btnAddRoute_onclick()" /><br />
    <br />
    <asp:Label ID="labContent" runat="server"></asp:Label>

    <script>

        var labContent = "<%= labContent.ClientID %>";
        function page_Load() {
            $$(labContent).showLoading("数据正在加载中。。");

            NetRadio.LocatingMonitor.Monitor.__RoutePatrol.GetHTML(function(r) {
                if (r.error == 0) {
                    $(labContent).innerHTML = r.value;
                }
                else {
                    $(labContent).innerHTML = r.errorText;
                }
            });
        }
        function btnAddRoute_onclick() {

        }

    </script>

</asp:Content>
