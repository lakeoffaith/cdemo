<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Member.__UserList" %>
<%@ Register TagPrefix="NetRadio" TagName="ObjectNavigator" Src="~/Controls/ObjectNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
<NetRadio:ObjectNavigator ID="objectNavigator" runat="server" />
<br />
    <div style="float: right;">
        <a href="EditUser.aspx?action=addnew" class="bullet_add" id="addUser" runat="server">
            新增用户</a></div>
    <table class="grid alternate fixed">
        <thead class="category">
            <th width="120">
                登录名
            </th>
            <th>
                角色
            </th>
            <th width="100" style="text-align: center">
                操作
            </th>
        </thead>
        <asp:Repeater ID="userRepeater" OnItemCreated="userRepeater_ItemCreated" runat="server">
            <ItemTemplate>
                <tr class="mouseOn">
                    <td>
                        <NetRadio:SmartLabel ID="userName" runat="server" />
                    </td>
                    <td>
                        <NetRadio:SmartLabel ID="userRole" runat="server" />
                    </td>
                    <td align="center">
                        <NetRadio:Anchor ID="edit" Text="编辑" class="underline" runat="server" /><span class="separator">
                            | </span>
                        <NetRadio:Anchor ID="delete" Text="删除" class="underline" runat="server" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
