<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagUserList1.ascx.cs"
    Inherits="NetRadio.LocatingMonitor.TagUsers.__TagUserList1" %>
<%@ Register Src="../Controls/SelectTag.ascx" TagName="SelectTag" TagPrefix="uc1" %>
<%@ Register src="../Controls/SelectStrongestRssiTag.ascx" tagname="SelectStrongestRssiTag" tagprefix="uc2" %>
<form id="form1" runat="server">
<uc2:SelectStrongestRssiTag ID="selectStrongestRssiTag" runat="server" EnableBorderCSS="true" EnableClose="true" EnableOK="true" EnableUnbind="true" />
<uc1:SelectTag ID="tagSelector" Model="Single" VisibleTip="false" runat="server" />
<table cellpadding="0" cellspacing="0" class="grid alternate fixed">
    <thead class="category">
        <th width="200">
            名称
        </th>
        <th style="text-align: center">
            编号
        </th>
        <th width="200" style="text-align: center">
            绑定标签
        </th>
        <th width="200" style="text-align: center">
            当前位置
        </th>
    </thead>
    <asp:Repeater ID="list" OnItemCreated="list_ItemCreated" OnItemDataBound="list_ItemDataBound"
        runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <NetRadio:Anchor ID="name" class="bold" Target="_detail" runat="server" /><NetRadio:Img
                        ID="tagIcon" Width="13" Height="13" runat="server" />
                </td>
                <td style="text-align: center">
                    <NetRadio:SmartLabel ID="number" Style="color: #990000" runat="server" />
                </td>
                <td style="text-align: center">
                    <NetRadio:SmartLabel ID="mac" Style="color: #990000" runat="server" /><NetRadio:Img ID="tagAlertIcon"
                        Width="16" Height="16" Href="" Target="new" runat="server" />
                    <NetRadio:Img ID="bindTag" Width="16" Height="16" Href="" Style="cursor: pointer;"
                        runat="server" />
                </td>
                <td style="text-align: center">
                    <NetRadio:Anchor ID="currentLocation" Style="color: #000099" Target="_detail" runat="server" /><NetRadio:Img
                        ID="tagDisappeared" Width="16" Height="16" Href="" Target="new" runat="server" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<div class="clear">
</div>
<div style="margin: 0 15px 0 15px">
    <NetRadio:Pagination ID="p" Mode="UserScript" class="p_left" PageSize="20" runat="server" />
</div>
</form>
