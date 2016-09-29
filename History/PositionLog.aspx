<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="PositionLog.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.History.__PositionLog" %>

<%@ Register TagPrefix="NetRadio" TagName="TagLogFilter" Src="~/Controls/TagLogFilter.ascx" %>
<%@ Register TagPrefix="NetRadio" TagName="HistoryNavigator" Src="~/Controls/HistoryNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <!-- Tab 导航 -->
    <NetRadio:HistoryNavigator id="historyNavigator" runat="server" />
    <div class="tabitem">
        <NetRadio:TagLogFilter id="tagLogFilter" FacilityFilterRowVisible="true" runat="server" />
        <NetRadio:CollapseView ID="listPanel" runat="server">
					<Header skinId="header_empty" />
					<Content>
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
										<td align="center"><NetRadio:Anchor id="replayRoute" class="underline" text="轨迹" title="回放前10分钟轨迹" runat="server" /></td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
							<tr>
								<td colspan="5"><NetRadio:Pagination id="p" pageSize="20" mode="Redirect" class="p_left" runat="server" /></td>
							</tr>
						</table>
					</Content>
        </NetRadio:CollapseView>
    </div>
</asp:Content>
