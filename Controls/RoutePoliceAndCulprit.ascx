<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoutePoliceAndCulprit.ascx.cs"
    Inherits="NetRadio.LocatingMonitor.Controls.__RoutePoliceAndCulprit" %>

<script language="javascript" type="text/javascript">
// <!CDATA[

    var RoutePoliceAndCulprit = {};

    RoutePoliceAndCulprit.show = function() {
        $("divRoutePoliceAndCulprit").style.display = "block";
        loadData();
    }
    RoutePoliceAndCulprit.close = function() {
        $("divRoutePoliceAndCulprit").style.display = "none";
    }

    RoutePoliceAndCulprit.AfterSaveRouteFun = function() { }
    function saveRoute_onclick() {

        var radioItem = $radio_GetCheckedRadio("name_police");
        var radioItem2 = $radio_GetCheckedRadio("name_culprit");
        var __options = $("listRight").options;
        var errorMes = "";
        if (radioItem == null) {
            errorMes += "请选择警察;\n";
        }

        if (radioItem2 == null) {
            errorMes += "请选择犯人;\n";
        }

        if (__options.length == 0) {
            errorMes += "请选择轨迹;\n";
        }
        if (errorMes.length > 0) {
            alert(errorMes);
            return false;
        }
        var cids = "";
        for (var i = 0; i < __options.length; i++) {
            if (i == 0)
                cids += __options[i].value;
            else
                cids += "," + __options[i].value;
        }

        NetRadio.LocatingMonitor.Controls.__RoutePoliceAndCulprit.UpdateData(parseInt(radioItem.value), parseInt(radioItem2.value), cids, function(r) {
            if (r.error == 0) {
                alert("..操作成功.." );//+ r.value

                if (RoutePoliceAndCulprit.AfterSaveRouteFun != null && typeof (RoutePoliceAndCulprit.AfterSaveRouteFun) == 'function') {
                    RoutePoliceAndCulprit.AfterSaveRouteFun();
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
        RoutePoliceAndCulprit.close();
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
    function id_culprit_onkeyup() {
        document.getElementById("divC").innerHTML = getHtmlCulprit(document.getElementById("id_culprit").value);
    }
    var g_police, g_culprit;

    function loadData() {
        NetRadio.LocatingMonitor.Controls.__RoutePoliceAndCulprit.GetData(function(r) {
            if (r.error == 0) {
                document.getElementById("div_listLeft").innerHTML = "<select id='listLeft' multiple size='15' style='width: 160px; height: 400px;'>" + r.value.positionList + "</select>";
                g_police = r.value.police;
                g_culprit = r.value.culprit;
                document.getElementById("divP").innerHTML = getHtmlPolice("");
                document.getElementById("divC").innerHTML = getHtmlCulprit("");
                document.getElementById("id_police").value = "";
                document.getElementById("id_culprit").value = "";
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
                htmlPolice.push(" <input type=radio name=name_police value=" + g_police[i].ID + " />" + g_police[i].Name + "<br />");
            }
            else if (g_police[i].Name.indexOf(_name) != -1) {
                htmlPolice.push(" <input type=radio name=name_police value=" + g_police[i].ID + " />" + g_police[i].Name + "<br />");
            }
        }
        return htmlPolice.join("");
    }

    function getHtmlCulprit(_name) {
        var htmlCulprit = [];
        for (var i = 0; i < g_culprit.length; i++) {
            if (_name.length == 0) {
                htmlCulprit.push(" <input type=radio name=name_culprit value=" + g_culprit[i].ID + " />" + g_culprit[i].Name + "<br />");
            }
            else if (g_culprit[i].Name.indexOf(_name) != -1) {
                htmlCulprit.push(" <input type=radio name=name_culprit value=" + g_culprit[i].ID + " />" + g_culprit[i].Name + "<br />");
            }
        }
        return htmlCulprit.join("");
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

<div id="divRoutePoliceAndCulprit" class="floatLayer" style="width: 810px; height: 510px;
    display: none;">
    <table border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="200" height="250" align="center" valign="top">
                警察列表<br />
                <div style="width: 180px; height: 230px; background-color: #D2E9FF; border: solid 1px #0079F2;
                    padding-left: 10px; text-align: left; overflow: hidden;">
                    <input id="id_police" type="text" onkeyup="id_police_onkeyup()" />
                    <div id="divP" style="height: 200px; overflow-y: scroll; overflow-x: hidden;">
                    </div>
                </div>
            </td>
            <td width="200" rowspan="2" align="center" valign="top">
                定位点列表<div style="width: 180px; height: 480px; background-color: #FFFFDD; border: solid 1px #009900;">
                    <br />
                    <div id="div_listLeft">
                    </div>
                </div>
            </td>
            <td width="100" rowspan="2" align="center" valign="middle">
                <input type="button" name="selectRight" id="selectRight" value="选择 >" onclick="return selectRight_onclick()" /><br />
                <br />
                <input type="button" name="selectLeft" id="selectLeft" value="选择 <" onclick="return selectLeft_onclick()" />
            </td>
            <td width="200" rowspan="2" align="center" valign="top">
                提讯轨迹<br />
                <div style="width: 180px; height: 480px; background-color: #FFFFDD; border: solid 1px #009900;">
                    <br />
                    <select id="listRight" multiple size="15" style="width: 160px; height: 400px;">
                    </select></div>
            </td>
            <td width="100" rowspan="2" align="center" valign="top">
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
        <tr>
            <td height="250" align="center" valign="top">
                犯人列表<br />
                <div style="width: 180px; height: 230px; background-color: #FFE2C6; border: solid 1px #FF9900;
                    padding-left: 10px; text-align: left; overflow: hidden;">
                    <input id="id_culprit" type="text" onkeyup="id_culprit_onkeyup()" />
                    <div id="divC" style="height: 200px; overflow-y: scroll; overflow-x: hidden;">
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
