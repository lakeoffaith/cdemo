<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="MapAreaRule.aspx.cs" Inherits="NetRadio.LocatingMonitor.Organize.__MapAreaRule" %>

<%@ Register Src="../Controls/SelectTagUser.ascx" TagName="SelectTagUser" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <NetRadio:FacilityMapDropList ID="maps" DisplayTreeStyle="false" DefaultText="所有地图"
        Style="width: 180px" runat="server" />
    <input id="btnAddRoute" type="button" value="筛选" onclick="return btn_onclick()" /><br />
    <uc1:SelectTagUser ID="tagSelector" runat="server" Visible_HrefSelectUser="false"
        Visible_selectedList="false" AutoLoadData="false" />
    <br />
    <asp:Label ID="labContent" runat="server"></asp:Label>

    <script>

        var labContent = "<%= labContent.ClientID %>";


        var _page_Load = page_Load;
        page_Load = function() {
            if (_page_Load != null && _page_Load != undefined) {
                _page_Load();
            }

            btn_onclick();
        }



        function btn_onclick() {
            $$(labContent).showLoading("数据正在加载中。。");
            var maps = '<%= maps.UniqueID%>';
            var mapID = document.getElementsByName(maps)[0][document.getElementsByName(maps)[0].selectedIndex].value;
            NetRadio.LocatingMonitor.Organize.__MapAreaRule.GetHTML(mapID, function(r) {
                if (r.error == 0) {
                    $(labContent).innerHTML = r.value;
                }
                else {
                    $(labContent).innerHTML = r.errorText;
                }
            });
        }


        var g_areaid, g_ruleid, g_areaInOut, g_tagIDs, g_forAll;
        function btnSet_onclick(areaid, ruleid, areaInOut, tagIDs) {
            SelectTagUser_ClearData();
            SelectTagUser_show();
            SelectTagUser_InitData();
            g_areaid = areaid;
            g_ruleid = ruleid;
            g_areaInOut = areaInOut;


            //alert(areaid + "-" + ruleid + "-" + areaInOut + "-" + tagIDs);
        }

        ok_onclick = function() {

            g_forAll = false;
            g_tagIDs = "";
            for (var i = 0; i < $("listRight").options.length; i++) {
                if (i == 0) {
                    g_tagIDs += $("listRight").options[i].TagID;
                }
                else {
                    g_tagIDs += "," + $("listRight").options[i].TagID;
                }
            }
            NetRadio.LocatingMonitor.Organize.__MapAreaRule.UpdateData(g_areaid, g_ruleid, g_areaInOut, g_tagIDs, g_forAll, function(r) {

                if (r.error == 0) {
                    if (r.value) {
                        g_ruleid = r.value.RuleID;
                        // alert('successful!');
                        $("loading").innerHTML = "√  操作成功";
                        window.setTimeout('$("loading").innerHTML ="";', 1000)
                        btn_onclick();
                    }
                }
                else {
                    $("loading").innerHTML = "×  操作失败，错误信息：" + r.errorText;
                    window.setTimeout('$("loading").innerHTML ="";', 1000)
                    //alert('failed!' + r.errorText);
                }
            });
            // alert(g_areaid + "-" + g_ruleid + "-" + g_areaInOut + "-" + g_tagIDs);
        }

    </script>

</asp:Content>
