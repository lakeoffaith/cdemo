<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="LowBatteryLog.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.History.__LowBatteryLog" %>

<%@ Register TagPrefix="NetRadio" TagName="TagLogFilter" Src="~/Controls/TagLogFilter.ascx" %>
<%@ Register TagPrefix="NetRadio" TagName="HistoryNavigator" Src="~/Controls/HistoryNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <!-- Tab 导航 -->
    <NetRadio:HistoryNavigator id="historyNavigator" runat="server" />
    <div class="tabitem">
        <NetRadio:TagLogFilter id="tagLogFilter" runat="server" />
        <NetRadio:CollapseView ID="listPanel" runat="server">
					<Header skinId="header_empty" />
					<Content>
						<table class="grid alternate fixed">
							<thead>
								<th width="180"><NetRadio:SortButton id="hostNameSorter" text="名称" sortKey="HostName" onclick="sorter_Click" runat="server" /></th>
								<th width="180">事件描述</th>
								<th>发生位置</th>
								<th width="135"><NetRadio:SortButton id="updateTimeSorter" text="发生时间" sortKey="WriteTime" onclick="sorter_Click" runat="server" /></th>
							</thead>
							<asp:Repeater id="list" onItemCreated="list_ItemCreated" runat="server">
								<ItemTemplate>
									<tr>
										<td><NetRadio:Anchor id="tagName" target="_blank" runat="server" /></td>
										<td>低电报警</td>
										<td><NetRadio:SmartLabel id="facilityName" runat="server" /><span class="t3">,</span> <NetRadio:SmartLabel id="coordinatesName" runat="server" /></td>
										<td><NetRadio:DateTimeLabel id="writeTime" showAsTimeSpan="false" format="yyyy-M-d H:mm:ss" runat="server" /></td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
							<tr>
								<td colspan="4"><NetRadio:Pagination id="p" pageSize="20" mode="Redirect" class="p_left" runat="server" /></td>
							</tr>
						</table>
					</Content>
        </NetRadio:CollapseView>
    </div>
</asp:Content>
