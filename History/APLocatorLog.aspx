<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="APLocatorLog.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.History.__APLocatorLog" %>

<%@ Register TagPrefix="NetRadio" TagName="ObjectNavigator" Src="~/Controls/ObjectNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <NetRadio:ObjectNavigator id="objectNavigator" runat="server" />
    <div class="tabitem">
        <NetRadio:CollapseView ID="searchResult" runat="server">
					<Header skinId="header_empty" />
					<Content>
						<table class="grid alternate fixed">
							<thead>
								<th width="240"><NetRadio:SortButton id="apNameSorter" text="AP定位器" sortKey="APName" onclick="sorter_Click" runat="server" /></th>
								<th width="120">事件描述</th>
								<th width="180">发生位置</th>
								<th width="135"><NetRadio:SortButton id="updateTimeSorter" text="发生时间" sortKey="WriteTime" onclick="sorter_Click" runat="server" /></th>
							</thead>
							<asp:Repeater id="list" onItemCreated="list_ItemCreated" runat="server">
								<ItemTemplate>
									<tr>
										<td><NetRadio:Anchor id="apName" target="_blank" runat="server" /></td>
										<td><NetRadio:Anchor id="apStatus" target="_blank" runat="server" /></td>
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
