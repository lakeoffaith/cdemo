<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProcessAlert.ascx.cs"
    Inherits="NetRadio.LocatingMonitor.Controls.__ProcessAlert" %>
<style>
    .preTd
    {
        font-size: 14px;
        color: Black;
        font-weight: bold;
    }
    .thisTable
    {
        background-color: Gray;
    }
    .thisTable tr td
    {
        background-color: White;
        height: 30px;
    }
    .yellowBG
    {
        background-color: #FFFFBF;
    }
    .yellowBG tr td
    {
        background-color: #FFFFBF;
    }
</style>
<div id="divLoading" style="font-size: 14px; display: block;">
    页面内容加载中。。。</div>
<table id="tabContent" style="display: none" border="0" cellspacing="1" cellpadding="0"
    width="100%" class="thisTable">
    <tr>
        <td width="100" rowspan="2" align="center" valign="middle" class="preTd">
            报警信息
        </td>
        <td>
            <div align="right">
                报警者名称:&nbsp;&nbsp;</div>
        </td>
        <td class="tdRed" id="id_name">
            &nbsp;
        </td>
        <td>
            <div align="right">
                报警位置:&nbsp;&nbsp;</div>
        </td>
        <td class="tdRed" id="id_position">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            <div align="right">
                报警类型:&nbsp;&nbsp;</div>
        </td>
        <td id="id_type" class="tdRed">
            &nbsp;
        </td>
        <td>
            <div align="right">
                报警时间:&nbsp;&nbsp;</div>
        </td>
        <td id="id_time" class="tdRed">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td align="center" valign="middle" class="preTd">
            处理报警
        </td>
        <td colspan="4" style="background-color: #FFFFBF;" class="yellowBG">
            <table width="614" border="0" cellspacing="0" cellpadding="0" class="yellowBG">
                <tr>
                    <td width="100" rowspan="3" align="center" valign="middle">
                        处理结果：
                    </td>
                    <td colspan="2" onclick="checkWhichSelect()">
                        <input type="radio" name="name_radiobutton" value="1" checked="checked" />
                        确认报警
                        <input type="radio" name="name_radiobutton" value="0" />
                        误报
                    </td>
                </tr>
                <tr>
                    <td width="204">
                        <select name="select" id="id_selectResult">
                        </select>
                    </td>
                    <td width="310">
                        <span style="color: #666666;">( 当确认报警时必选 )</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input id="ButOK" type="button" value=" 提 交 " onclick="return ButOK_onclick()" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="5" align="center" class="preTd">
            报警处理进度表
        </td>
    </tr>
    <tr>
        <td colspan="5" id="id_table">
        </td>
    </tr>
</table>

<script>


    var ProcessAlert = {};
    ProcessAlert.AlertID = parseInt(request("id"));
    if (ProcessAlert.AlertID == NaN || ProcessAlert.AlertID.toString().length == 0) {
        ProcessAlert.AlertID = 0;
    }

    ProcessAlert.AlertID =<%= this.AlertID %>;
    //ProcessAlert.AlertID = 936;
    window.onload=function() {
        NetRadio.LocatingMonitor.Controls.__ProcessAlert.GetData(ProcessAlert.AlertID, function(r) {
            if (r.error == 0) {
                $("id_name").innerHTML = r.value.id_name;
                $("id_position").innerHTML = r.value.id_position;
                $("id_type").innerHTML = r.value.id_type;
                $("id_time").innerHTML = r.value.id_time;
                $("id_table").innerHTML = r.value.id_table;

                $select_ClearOptions("id_selectResult");
                $select_AddOneOption("id_selectResult", "==请选择处理结果==", "0");

                for (var i = 0; i < r.value.id_selectResult.length; i++) {
                    $select_AddOneOption("id_selectResult", r.value.id_selectResult[i].Name, r.value.id_selectResult[i].ID);
                }

                $("divLoading").style.display = "none";
                $("tabContent").style.display = "block";
            }
            else {
                alert(r.errorText);
            }
        });
    }
    
    function checkWhichSelect()
    { 
        if( parseInt($radio_GetCheckedRadio("name_radiobutton").value)==0)
        {
          $("id_selectResult").selectedIndex=0;
          $("id_selectResult").disabled=true;
        }
        else{
         $("id_selectResult").disabled=false;
        }
    }
    
    function ButOK_onclick() {
        if ($radio_GetCheckedRadio("name_radiobutton").value == 1) {
            if ($select_GetSelectedOption("id_selectResult").value == 0) {
                alert("请选择处理结果！");
                return 0;
            }
        }

        var _value, processID,processName;
        _value = parseInt($radio_GetCheckedRadio("name_radiobutton").value);
        processID = parseInt($select_GetSelectedOption("id_selectResult").value);
        processName = $select_GetSelectedOption("id_selectResult").text ;

        NetRadio.LocatingMonitor.Controls.__ProcessAlert.ProcessAlertFun(ProcessAlert.AlertID, _value, processID, processName,function(r) {
            if (r.error == 0) {
                alert("...操作成功...");
                NetRadio.LocatingMonitor.Controls.__ProcessAlert.GetProcessTable(ProcessAlert.AlertID, function(r) {
                    if (r.error == 0) {
                        $("id_table").innerHTML = r.value;
                    }
                    else {
                        alert(r.errorText);
                    }
                });
            }
            else {
                alert(r.errorText);
            }
        });
    }

</script>

