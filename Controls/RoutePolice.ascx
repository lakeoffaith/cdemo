<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoutePolice.ascx.cs"
    Inherits="NetRadio.LocatingMonitor.Controls.__RoutePolice" %>

<script language="javascript" type="text/javascript">
// <!CDATA[

    var RoutePolice = {};

    RoutePolice.show = function() {
        $("divRoutePolice").style.display = "block";
        loadData();
    }
    RoutePolice.close = function() {
        $("divRoutePolice").style.display = "none";
    }
    RoutePolice.AfterSaveRouteFun = function() { }
    function saveRoute_onclick() {

        var __checkboxs = $checkbox_GetCheckedBoxs("name_police");
        var __options = $("listRight").options;
        var errorMes = "";
        if (__checkboxs == null || __checkboxs == undefined || __checkboxs.length == 0) {
        
            errorMes += "请选择警察;\n";
        }
        if (__options.length == 0) {
            errorMes += "请选择轨迹;\n";
        }
        if (errorMes.length > 0) {
            alert(errorMes);
            return false;
        }
        var cids = "", pids = "";
        for (var i = 0; i < __checkboxs.length; i++) {
            if (i == 0)
                pids += __checkboxs[i].value;
            else
                pids += "," + __checkboxs[i].value;
        }
        for (var i = 0; i < __options.length; i++) {
            if (i == 0)
                cids += __options[i].value;
            else
                cids += "," + __options[i].value;
        }



        NetRadio.LocatingMonitor.Controls.__RoutePolice.UpdateData(pids, cids, function(r) {
            if (r.error == 0) {
                alert("..操作成功.." );//+ r.value
                if (RoutePolice.AfterSaveRouteFun != null && typeof (RoutePolice.AfterSaveRouteFun) == 'function') {
                    RoutePolice.AfterSaveRouteFun();
                }
            }
            else {
                alert(r.errorText);
            }
        });
    }

    function resetBtn_onclick() {
        loadData();
    }

    function closeWin_onclick() {
        RoutePolice.close();
    }

    function selectRight_onclick() {
        multi_onclick("listLeft", "listRight");
    }

    function selectLeft_onclick() {
        multi_onclick("listRight", "listLeft");
    }
    function id_police_onkeyup() {
        document.getElementById("divP").innerHTML = getHtmlPolice(document.getElementById("id_police").value);
    }
    var g_police;

    function loadData() {
        NetRadio.LocatingMonitor.Controls.__RoutePolice.GetData(function(r) {
            if (r.error == 0) {
                document.getElementById("div_listLeft").innerHTML = "<select id='listLeft' multiple size='15' style='width: 160px; height: 400px;'>" + r.value.positionList + "</select>";
                g_police = r.value.police;
                document.getElementById("divP").innerHTML = getHtmlPolice("");
                document.getElementById("id_police").value = "";
                $select_ClearOptions("listRight");
            }
            else {
                alert(r.errorText);
            }
        });
    }

    function getHtmlPolice(_name) {
        var htmlPolice = [];
        for (var i = 0; i < g_police.length; i++) {
            if (_name.length == 0) {
                htmlPolice.push(" <input type=checkbox name=name_police value=" + g_police[i].ID + " />" + g_police[i].Name + "<br />");
            }
            else if (g_police[i].Name.indexOf(_name) != -1) {
                htmlPolice.push(" <input type=checkbox name=name_police value=" + g_police[i].ID + " />" + g_police[i].Name + "<br />");
            }
        }
        return htmlPolice.join("");
    }


    function isExists(id, value) {
        for (var i = $(id).options.length - 1; i >= 0; i--) {
            if ($(id).options[i].value == value) {
                return true;
            }
        }
        return false;
    }

    function multi_onclick(from, to) {
        for (var i = $(from).options.length - 1; i >= 0; i--) {
            if ($(from).options[i].selected) {
                var item = document.createElement("option");
                item.text = $(from).options[i].text;
                item.value = $(from).options[i].value;
                item.ID = $(from).options[i].ID;
                if (!isExists(to, item.value)) {
                    $select_AddOption(to, item);
                }
                $select_DeleteOption(from, $(from).options[i].index);
            }
        }
    }
// ]]>
</script>

<div id="divRoutePolice" class="floatLayer" style="width: 810px; height: 510px; display: none;">
    <table border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="200" align="center" valign="top">
                警察列表<br />
                <div style="width: 180px; height: 480px; background-color: #D2E9FF; border: solid 1px #0079F2;
                    padding-left: 10px; text-align: left; overflow: hidden;">
                    <input id="id_police" type="text" onkeyup="id_police_onkeyup()" />
                    <div id="divP" style="height: 420px; overflow-y: scroll; overflow-x: hidden;">
                    </div>
                </div>
                <br />
            </td>
            <td width="200" align="center" valign="top">
                定位点列表<div style="width: 180px; height: 480px; background-color: #FFFFDD; border: solid 1px #009900;">
                    <br />
                    <div id="div_listLeft">
                    </div>
                </div>
            </td>
            <td width="100" align="center" valign="middle">
                <input type="button" name="selectRight" id="selectRight" value="选择 >" onclick="return selectRight_onclick()" /><br />
                <br />
                <input type="button" name="selectLeft" id="selectLeft" value="选择 <" onclick="return selectLeft_onclick()" />
            </td>
            <td width="200" align="center" valign="top">
                巡逻轨迹<br />
                <div style="width: 180px; height: 480px; background-color: #FFFFDD; border: solid 1px #009900;">
                    <br />
                    <select id="listRight" multiple size="15" style="width: 160px; height: 400px;">
                    </select></div>
            </td>
            <td width="100" align="center" valign="top">
                <label>
                    <br />
                    <br />
                    <input type="button" name="saveRoute" id="saveRoute" value="保存轨迹" onclick="return saveRoute_onclick()"
                        class="button" /><br />
                    <br />
                    <input type="button" name="resetBtn" id="resetBtn" value="重置" onclick="return resetBtn_onclick()"
                        class="button" /><br />
                    <br />
                    <input type="button" name="closeWin" id="closeWin" value="关闭" onclick="return closeWin_onclick()"
                        class="button" />
                </label>
            </td>
        </tr>
    </table>
</div>
