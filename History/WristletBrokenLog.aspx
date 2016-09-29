<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="WristletBrokenLog.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.History.__WristletBrokenLog" %>

<%@ Register TagPrefix="NetRadio" TagName="HistoryNavigator" Src="~/Controls/HistoryNavigator.ascx" %>
<%@ Register Src="../Controls/TagLogFilter.ascx" TagName="TagLogFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <!-- Tab 导航 -->
    <NetRadio:HistoryNavigator ID="historyNavigator1" runat="server" />
    <div class="tabitem">
        <uc1:TagLogFilter ID="TagLogFilter1" runat="server" />
        <NetRadio:CollapseView ID="CollapseView1" runat="server">
					<Header skinId="header_empty" />
					<Content>
						<table class="grid alternate fixed">
							<thead>
								<th width="180">标签名称</th>
								<th width="180">事件描述</th>
								<th>发生位置</th>
								<th width="135">时间</th>
							</thead>
							<asp:Repeater  id="list"  onItemCreated="list_ItemCreated" runat="server">
								<ItemTemplate>
									<tr>
										<td><NetRadio:Anchor id="tagName" target="_blank" runat="server" /></td>
										<td>腕带断开</td>
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
