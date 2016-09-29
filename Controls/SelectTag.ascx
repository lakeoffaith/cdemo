<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectTag.ascx.cs" Inherits="NetRadio.LocatingMonitor.Controls.__SelectTag" %>
<a id="selectorLauncher" href="javascript:void(0);" onclick="javascript:SelectTag.Show();"
    class="underline" style="width: 70px; display: <%= VisibleTip? "block":"none"%>;">
    选择标签...</a>
<div id="divSelectTag" class="floatLayer" style="width: 650px; height: 410px; display: none;">
   
    <input id="hidModel" name="hidModel" type="hidden" value="<%= (int)Model%>" />
    <input id="hidSelectedTagNames" name="hidSelectedTagNames" type="hidden" value="" />
    <input id="hidSelectedTagIDs" name="hidSelectedTagIDs" type="hidden" value="" />
    <input id="hidSelectedTagMACs" name="hidSelectedTagMACs" type="hidden" value="" />
    <input id="hidEnableOkFunction" name="hidEnableOkFunction" type="hidden" value="<%= EnableOkFunction%>" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <input id="keyWord" type="text" style="width: 80px; color: Gray;" value="关键字" onfocus="javascript:__userControl.keywordFocus(this);"
                    onblur="javascript:__userControl.keywordBlur(this);" />
                <input type="button" value="搜索" style="width: 50px;" onclick="return searchTags()" />
                <select id="selectPage" onchange="selectPage_onchange()">
                    <option value="1" >第1页</option>
                </select>
            </td>
        </tr>
        <tr>
            <td height="40">
                <div style="height:40px; width:650px; margin: 0px;overflow:auto">
                    已选择的标签( 共<span style="color: red;" id="span_SelectedTagsCount">0</span>个 )：<span
                        style="color: Gray" id="span_SelectedTags"></span>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div id="tags" style="height: 310px; margin: 0px; overflow-y: scroll;">
                </div>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px;">
                <div id="loading" style="float: left;">
                </div>
                <div style="float: right;">
                    <input id="butUnbind" type="button" value=" 解除 " style="display: <%= VisibleUnBindButton? "inline":"none"%>;" onclick="return unbind_onclick()" />
                    <input id="butOK" type="button" value=" 确定 " style="margin-left: 20px;" onclick="return ok_onclick()" />
                    <input id="butCancel" type="button" value=" 关闭 " style="margin-left: 20px;" onclick="return cancel_onclick()" />
                </div>
            </td>
        </tr>
    </table>
</div>
<div id="selectedList" class="hostList" style="display: <%= VisibleTip? "block":"none"%>;">
</div>
