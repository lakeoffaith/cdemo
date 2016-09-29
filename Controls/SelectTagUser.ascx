
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectTagUser.ascx.cs"
    Inherits="NetRadio.LocatingMonitor.Controls.__SelectTagUser" %>

<script language="javascript" type="text/javascript" src="../App_Script/Control.js"></script>

<script language="javascript" type="text/javascript">
// <!CDATA[
var ID_selectedGroupName='<%= selectedGroupName.ClientID%>';
var ID_maxLength='<%= maxLength.ClientID%>';
var ID_methodGetUserRight='<%= methodGetUserRight.ClientID %>';
var ID_methodGetUserLeft='<%= methodGetUserLeft.ClientID %>';
var ID_selectedUserIds = '<%= selectedUserIds.ClientID %>';

var ID_selectorLauncher = '<%= selectorLauncher.ClientID %>';
var ID_selectedList = '<%= selectedList.ClientID %>';
// ]]>
var Para_AutoLoadData = <%= AutoLoadData.ToString().ToLower() %>;


function SelectTagUser_show() {
    __userControl.show('divSelectTagUser');

}
function SelectTagUser_IDs()
{
 return $(ID_selectedUserIds).value==null? "": $(ID_selectedUserIds).value;
}
</script>

<asp:HiddenField ID="maxLength" runat="server" Value="" />
<asp:HiddenField ID="methodGetUserLeft" runat="server" Value="" />
<asp:HiddenField ID="methodGetUserRight" runat="server" Value="" />
<asp:HiddenField ID="selectedUserIds" runat="server" Value="null" />
<a id="selectorLauncher" runat="server" href="javascript:void(0);" onclick="javascript:SelectTagUser_show();"
    class="underline" style="width: 70px;">选择对象...</a>
<div id="divSelectTagUser" class="floatLayer" style="width: 595px; height: 410px;
    display: none">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td height="25">
                <asp:Label ID="labLeft" runat="server" Text="◆◆ 所有分组对象" Style="font-size: 13px; font-weight: bold;"></asp:Label>
            </td>
            <td align="center" width="100">
                &nbsp;
            </td>
            <td colspan="2">
                <asp:Label ID="labRight" runat="server" Text="◆◆ 本组已选对象" Style="font-size: 13px;
                    font-weight: bold;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="230" height="30">
                <input id="tagNameKeyword" type="text" style="width: 80px; color: Gray;" value="关键字"
                    style="color: gray" onfocus="javascript:__userControl.keywordFocus(this);" onblur="javascript:__userControl.keywordBlur(this);" />
                <div runat="server" id="divGroup" style="display: inline;">
                    <NetRadio:TagGroupSelector ID="selectedGroupName" Width="80" runat="server" />
                </div>
                <input type="button" value="搜索" style="width: 50px;" onclick="return searchLeft_onclick()" />
            </td>
            <td align="center" width="100">
                &nbsp;
            </td>
            <td width="230" colspan="2">
                <input id="keyWord" type="text" style="width: 80px; color: Gray;" value="关键字" style="color: gray"
                    onfocus="javascript:__userControl.keywordFocus(this);" onblur="javascript:__userControl.keywordBlur(this);" />
                <input type="button" value="搜索" style="width: 50px;" onclick="return searchRight_onclick()" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <select id="listLeft" multiple size="15" style="width: 160px;">
                </select>
            </td>
            <td align="center" style="overflow: visible;">
                <input type="button" value="选择 >" style="width: 80px; margin-bottom: 5px; position: relative;
                    left: -35px;" onclick="return __some()" /><br />
                <input type="button" value="全部 >" style="width: 80px; margin-bottom: 5px; position: relative;
                    left: -35px;" onclick="return __all()" /><br />
                <br />
                <input type="button" value="< 选择" style="width: 80px; margin-bottom: 5px; position: relative;
                    left: -35px;" onclick="return multi_onclick('listRight','listLeft')" /><br />
                <input type="button" value="< 全部" style="width: 80px; margin-bottom: 5px; position: relative;
                    left: -35px;" onclick="return total_onclick('listRight','listLeft')" /><br />
            </td>
            <td align="left">
                <select id="listRight" multiple size="15" style="width: 160px;">
                </select>
            </td>
            <td align="center">
                <input type="button" value=" 确定 " onclick="return ok_onclick()" /><br />
                <br />
                <br />
                <input type="button" value=" 关闭 " style="margin-left: 0px;" onclick="return cancel_onclick()" />
            </td>
        </tr>
        <tr>
            <td width="230" style="padding-left: 50px;" height="30">
                共计 <font color="red" id="lCount">0</font> 个
            </td>
            <td>
                &nbsp;
            </td>
            <td width="230" colspan="2" style="padding-left: 50px;">
                共计 <font color="red" id="rCount">0</font> 个
            </td>
        </tr>
        <tr>
            <td colspan="4" id="loading" style="color: Red; font-size: 14px;">
            </td>
        </tr>
    </table>
</div>
<div runat="server" id="selectedList" class="hostList" style="display: none;">
</div>
