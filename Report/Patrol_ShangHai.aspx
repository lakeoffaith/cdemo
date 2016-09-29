<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="Patrol_ShangHai.aspx.cs" Inherits="NetRadio.LocatingMonitor.Organize.__Patrol_ShangHai" %>

<%@ Register TagPrefix="NetRadio" TagName="SelectTagUser" Src="~/Controls/SelectTagUser.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <input type="button" value="设置查询条件" onclick="return btn_onclick()" /><br />

    <script>

        function __callFilter() {
            var layer = $$("filterForm");
            layer.display();
            layer.locateCenter();
        }

        function __cancelFilter() {
            $$("filterForm").hide();

        }
        function searchData_Click() {
            getHtml();
            return false;
        }
    </script>

    <div id="filterForm" class="floatLayer" style="cursor: pointer; display: none">
        <table width="425" cellpadding="0" cellspacing="0">
            <tr>
                <td class="t1 bold bigger">
                    设置查询条件
                </td>
                <td align="right">
                    <a href="javascript:__cancelFilter();" class="underline">取消</a>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding: 15px 0">
                    <table class="grid fixed">
                        <tr>
                            <td width="135">
                                查找的名称:
                            </td>
                            <td>
                                <NetRadio:SmartLabel ID="nameCalling" runat="server" /><NetRadio:SelectTagUser ID="tagSelector"
                                    runat="server" TitleRight="◆◆ 已选对象" Visible_divGroup="false" />
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                时间范围从:
                            </td>
                            <td>
                                <NetRadio:CalendarBox ID="fromDate" Width="90" runat="server" />
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
                            </td>
                        </tr>
                        <tr>
                            <td>
                                时间范围至:
                            </td>
                            <td>
                                <NetRadio:CalendarBox ID="toDate" Width="90" runat="server" />
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
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="searchButton" Text="提交查询" Width="120" runat="server" OnClientClick="return searchData_Click();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <asp:Label ID="labContent" runat="server"></asp:Label>

    <script>

        var labContent = "<%= labContent.ClientID %>";



        window.onload = function() {
            getHtml();
        }
        function btn_onclick() {
            __callFilter();
        }


        function getHtml() {
            var userIDs = SelectTagUser_IDs();
            var beginTime = $('fromDate').value + " " + $('fromHour').value + ":00:00";
            var endTime = $('toDate').value + " " + $('toHour').value + ":59:59";
            NetRadio.LocatingMonitor.Organize.__Patrol_ShangHai.GetHTML(gPageIndex, userIDs, beginTime, endTime, function(r) {

                if (r.error == 0) {
                    $(labContent).innerHTML = r.value;
                    __cancelFilter();
                }
                else {
                    alert('failed! ' + r.errorText);
                }
            });
            // alert(g_areaid + "-" + g_ruleid + "-" + g_areaInOut + "-" + g_tagIDs);
        }



        var gPageIndex = 0;
        function pagerScriptLinkButton_click(id, pageIndex) {
            gPageIndex = pageIndex;
            getHtml();
        }	

    </script>

</asp:Content>
