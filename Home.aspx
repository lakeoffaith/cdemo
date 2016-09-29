<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.__Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <%
     
        switch (NetRadio.Business.BusAppInfo.Theme)
        {
            case "Prison":
    %>
    <!-- #include file="/Controls/home1.htm" -->
    <%
        break;
            case "Default":
            default:
    %>
    <!-- #include file="/Controls/home0.htm" -->
    <%
        break;
        }
    %>

    <script>

        window.onload = function() {
            getHtml();
        }

        function getHtml() {
            NetRadio.LocatingMonitor.__Home.GetHTML(function(r) {
                if (r.error == 0) {
                    $("id_people").innerHTML = r.value.id_people;
                    $("id_btn").innerHTML = r.value.id_btn;
                    $("id_lowpower").innerHTML = r.value.id_lowpower;
                    $("id_reset").innerHTML = r.value.id_reset;
                    $("id_area").innerHTML = r.value.id_area;
                    $("id_ap").innerHTML = r.value.id_ap;
                    $("id_goodap").innerHTML = r.value.id_goodap;
                }
                else {
                    alert('failed! ' + r.errorText);
                }
            });
            // alert(g_areaid + "-" + g_ruleid + "-" + g_areaInOut + "-" + g_tagIDs);
        }
    </script>

</asp:Content>
