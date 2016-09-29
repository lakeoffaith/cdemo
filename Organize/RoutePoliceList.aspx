<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="RoutePoliceList.aspx.cs" Inherits="NetRadio.LocatingMonitor.Organize.__RoutePoliceList" %>

<%@ Register Src="../Controls/RoutePolice.ascx" TagName="RoutePolice" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <input type="button" value="增加巡逻轨迹" onclick="return btn_onclick()" /><br />
    <uc1:RoutePolice ID="RoutePolice1" runat="server" />
    <br />
    <asp:Label ID="labContent" runat="server"></asp:Label>

    <script>

        var labContent = "<%= labContent.ClientID %>";



        window.onload = function() {
            getHtml();
        }
        function btn_onclick() {
            RoutePolice.show();
        }

        function __delete(id) {
            NetRadio.LocatingMonitor.Organize.__RoutePoliceList.DeleteData(id, function(r) {
                if (r.error == 0) {
                    if (r.value) {
                        getHtml();
                    }
                }
                else {
                    alert('failed! ' + r.errorText);
                }
            });
        }
        function getHtml() {
            NetRadio.LocatingMonitor.Organize.__RoutePoliceList.GetHTML(gPageIndex,function(r) {

                if (r.error == 0) {
                    $(labContent).innerHTML = r.value;
                }
                else {
                    alert('failed! ' + r.errorText);
                }
            });
            // alert(g_areaid + "-" + g_ruleid + "-" + g_areaInOut + "-" + g_tagIDs);
        }

        RoutePolice.AfterSaveRouteFun = function() {
            getHtml();
            RoutePolice.close();
        }

        var gPageIndex = 0;
        function pagerScriptLinkButton_click(id, pageIndex) {
            gPageIndex = pageIndex;
            getHtml();
        }	

    </script>

</asp:Content>
