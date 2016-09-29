<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="PoliceStayTimeUI.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Report.__PoliceStayTimeUI" %>

<%@ Register TagPrefix="NetRadio" TagName="TagUserSelector" Src="~/Controls/TagUserSelector.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <blockquote>
        <table class="fixed grid">
            <tr id="areaSelecting" style="display: none">
                <td width="130">
                    选择区域
                </td>
                <td>
                    <asp:DropDownList ID="areaList" Width="140" runat="server" />
                </td>
            </tr>
            <tr width="130">
                <td>
                    选择使用者
                </td>
                <td>
                    <NetRadio:TagUserSelector id="tagUserSelector" SelectedTagsVisible="true" UserType="Cop"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    选择时间范围
                </td>
                <td>
                    <NetRadio:CalendarBox ID="fromDate" Width="80" runat="server" />
                    <asp:DropDownList ID="fromHour" runat="server">
                        <asp:ListItem Value="0">0 时</asp:ListItem>
                        <asp:ListItem Value="1">1 时</asp:ListItem>
                        <asp:ListItem Value="2">2 时</asp:ListItem>
                        <asp:ListItem Value="3">3 时</asp:ListItem>
                        <asp:ListItem Value="4">4 时</asp:ListItem>
                        <asp:ListItem Value="5">5 时</asp:ListItem>
                        <asp:ListItem Value="6">6 时</asp:ListItem>
                        <asp:ListItem Value="7">7 时</asp:ListItem>
                        <asp:ListItem Value="8">8 时</asp:ListItem>
                        <asp:ListItem Value="9">9 时</asp:ListItem>
                        <asp:ListItem Value="10">10 时</asp:ListItem>
                        <asp:ListItem Value="11">11 时</asp:ListItem>
                        <asp:ListItem Value="12">12 时</asp:ListItem>
                        <asp:ListItem Value="13">13 时</asp:ListItem>
                        <asp:ListItem Value="14">14 时</asp:ListItem>
                        <asp:ListItem Value="15">15 时</asp:ListItem>
                        <asp:ListItem Value="16">16 时</asp:ListItem>
                        <asp:ListItem Value="17">17 时</asp:ListItem>
                        <asp:ListItem Value="18">18 时</asp:ListItem>
                        <asp:ListItem Value="19">19 时</asp:ListItem>
                        <asp:ListItem Value="20">20 时</asp:ListItem>
                        <asp:ListItem Value="21">21 时</asp:ListItem>
                        <asp:ListItem Value="22">22 时</asp:ListItem>
                        <asp:ListItem Value="23">23 时</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="fromMinute" value="00" Width="25" MaxLength="2" runat="server" />
                    &nbsp; — &nbsp;
                    <NetRadio:CalendarBox ID="toDate" Width="80" runat="server" />
                    <asp:DropDownList ID="toHour" runat="server">
                        <asp:ListItem Value="0">0 时</asp:ListItem>
                        <asp:ListItem Value="1">1 时</asp:ListItem>
                        <asp:ListItem Value="2">2 时</asp:ListItem>
                        <asp:ListItem Value="3">3 时</asp:ListItem>
                        <asp:ListItem Value="4">4 时</asp:ListItem>
                        <asp:ListItem Value="5">5 时</asp:ListItem>
                        <asp:ListItem Value="6">6 时</asp:ListItem>
                        <asp:ListItem Value="7">7 时</asp:ListItem>
                        <asp:ListItem Value="8">8 时</asp:ListItem>
                        <asp:ListItem Value="9">9 时</asp:ListItem>
                        <asp:ListItem Value="10">10 时</asp:ListItem>
                        <asp:ListItem Value="11">11 时</asp:ListItem>
                        <asp:ListItem Value="12">12 时</asp:ListItem>
                        <asp:ListItem Value="13">13 时</asp:ListItem>
                        <asp:ListItem Value="14">14 时</asp:ListItem>
                        <asp:ListItem Value="15">15 时</asp:ListItem>
                        <asp:ListItem Value="16">16 时</asp:ListItem>
                        <asp:ListItem Value="17">17 时</asp:ListItem>
                        <asp:ListItem Value="18">18 时</asp:ListItem>
                        <asp:ListItem Value="19">19 时</asp:ListItem>
                        <asp:ListItem Value="20">20 时</asp:ListItem>
                        <asp:ListItem Value="21">21 时</asp:ListItem>
                        <asp:ListItem Value="22">22 时</asp:ListItem>
                        <asp:ListItem Value="23">23 时</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="toMinute" value="00" Width="25" MaxLength="2" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <input type="button" value="查看统计" class="button" onclick="javascript:viewAreaStayTimeReport();" />
                </td>
            </tr>
        </table>
    </blockquote>
</asp:Content>
