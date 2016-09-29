<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="PatrolReport.aspx.cs" Inherits="NetRadio.LocatingMonitor.Monitor.__PatrolReport" %>

<%@ Register TagPrefix="NetRadio" TagName="SelectTagUser" Src="~/Controls/SelectTagUser.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">

    <script>

        function __callFilter() {
            var layer = $$("filterForm");
            layer.display();
            layer.locateCenter();
        }

        function __cancelFilter() {
            $$("filterForm").hide();

        }
    </script>

    <div style="margin: 10px auto">
        <div class="fl">
            <input type="button" value="设置查询条件" class="button" style="width: 120px" onclick="javascript:__callFilter();" /></div>
        <div class="fr">
            <NetRadio:SmartLabel ID="condtionDescription" runat="server" /></div>
        <div class="clear">
        </div>
    </div>
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
                                <asp:Button ID="searchButton" Text="提交查询" Width="120" runat="server" OnClick="searchData_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div class="tabitem">
        <%--<NetRadio:TagLogFilter id="tagLogFilter" FacilityFilterRowVisible="true" runat="server" />--%>
        <NetRadio:CollapseView ID="listPanel" runat="server">
					<Header skinId="header_empty" />
					<Content>
					        <NetRadio:SmartLabel id="policeNames" runat="server" />
                   
					
						<table class="grid alternate fixed">
							<thead>
								<th width="16"></th>
								<th width="180"><NetRadio:SortButton id="hostNameSorter" text="名称" sortKey="HostName" onclick="sorter_Click" runat="server" /></th>
								<th>移动至位置</th>
								<th width="60">是否消失</th>
								<th width="135"><NetRadio:SortButton id="updateTimeSorter" text="发生时间" sortKey="WriteTime" onclick="sorter_Click" runat="server" /></th>
								<th width="80" style="text-align:center">轨迹回放</th>
							</thead>
							<asp:Repeater id="list" onItemCreated="list_ItemCreated" runat="server">
								<ItemTemplate>
									<tr>
										<td><NetRadio:Img id="icon" runat="server" /></td>
										<td><NetRadio:Anchor id="tagName" target="_blank" runat="server" /></td>
										<td><NetRadio:SmartLabel id="facilityName" runat="server" /> &nbsp;-&nbsp; <NetRadio:SmartLabel id="coordinatesName" runat="server" /></td>
										<td><NetRadio:SmartLabel id="isDisappeared" runat="server" /></td>
										<td><NetRadio:DateTimeLabel id="writeTime" showAsTimeSpan="false" format="yyyy-M-d H:mm:ss" runat="server" /></td>
										<td align="center"><NetRadio:Anchor id="replayRoute" class="underline" text="轨迹" title="回放前3分钟轨迹" runat="server" /></td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
							<tr>
								<td colspan="5"><NetRadio:Pagination id="p" pageSize="20" Mode="Postback" onpageindexchanged="p_PageIndexChanged" class="p_left" runat="server" /></td>
							</tr>
						</table>
					</Content>
        </NetRadio:CollapseView>
    </div>
</asp:Content>
