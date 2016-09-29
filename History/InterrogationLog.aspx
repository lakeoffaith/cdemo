<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="InterrogationLog.aspx.cs" Inherits="NetRadio.LocatingMonitor.History.__InterrogationLog" %>

<%@ Register TagPrefix="NetRadio" TagName="HistoryNavigator" Src="~/Controls/HistoryNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <!-- Tab 导航 -->
    <NetRadio:HistoryNavigator id="historyNavigator" runat="server" />
    <div class="tabitem">
        <NetRadio:CollapseView ID="collapseView" runat="server">
			<Content>
			    <table class="grid alternate fixed">
			        <thead>
						<th width="150">提审干警</th>
						<th width="150">被提审人</th>
						<th>状态</th>
						<th width="150">开始时间</th>
						<th width="150">结束时间</th>
					</thead>
						<asp:Repeater id="repeater" onitemcreated="repeater_ItemCreated" runat="server">
							<ItemTemplate>
								<tr>
									<td><NetRadio:SmartLabel id="policeName" runat="server" /></td>
									<td><NetRadio:SmartLabel id="culpritName" target="_blank" runat="server" /></td>
									<td><NetRadio:SmartLabel id="status" target="_blank" runat="server" /></td>
									<td><NetRadio:DateTimeLabel id="StartTime"  format="yyyy-M-d H:mm:ss" showastimespan="false" runat="server"  /></td>
									<td><NetRadio:DateTimeLabel id="EndTime" format="yyyy-M-d H:mm:ss" showastimespan="false" runat="server" /></td>
								</tr>
							</ItemTemplate>
						</asp:Repeater>
						<tr>
							<td colspan="5"><NetRadio:Pagination id="p" mode="PostBack" pagesize="50" class="p_left" onpageindexchanged="p_PageIndexChanged" runat="server" /></td>
						</tr>
			    </table>
			</Content>
        </NetRadio:CollapseView>
    </div>
</asp:Content>
