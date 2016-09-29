<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="LatestEvents.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Monitor.__LatestEvents" %>

<%@ Register TagPrefix="NetRadio" TagName="TagLogFilter" Src="~/Controls/TagLogFilter.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <asp:ScriptManager ID="clientScriptManager" runat="server" />
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <NetRadio:TagLogFilter ID="tagLogFilter" FacilityFilterRowVisible="true" runat="server" />
            <NetRadio:CollapseView ID="collapseView" runat="server">
						<Content>
							<table class="grid alternate fixed">
								<thead>
									<th width="35"></th>
									<th width="120"><NetRadio:SortButton id="hostNameSorter" text="名称" sortKey="HostName" onclick="sorter_Click" runat="server" /></th>
									<th width="120">发生场地</th>
									<th width="100">发生位置</th>
									<th width="150">警告或事件类型</th>
									<th>状态</th>
									<th width="130"><NetRadio:SortButton id="updateTimeSorter" text="发生时间" sortKey="WriteTime" onclick="sorter_Click" runat="server" /></th>
									<th width="50">详情</th>
								</thead>
								<asp:Repeater id="messageRepeater" onitemcreated="messageRepeater_ItemCreated" runat="server">
									<ItemTemplate>
										<tr id="tr" runat="server">
											<td align="center"><NetRadio:Anchor id="isResolved" runat="server" visble="false"/></td>
											<td><asp:Image id="icon" runat="server"></asp:Image><NetRadio:Anchor id="tagName" target="_blank" runat="server" /></td>
											<td><NetRadio:SmartLabel ID="facilityName" runat="server"></NetRadio:SmartLabel> </td>
											<td><NetRadio:SmartLabel id="coordinatesName" runat="server" /></td>
											<td><NetRadio:SmartLabel id="eventType" runat="server" /></td>
											<td><NetRadio:SmartLabel id="alertStatus" runat="server" /></td>
											<td><NetRadio:DateTimeLabel id="lastHappenTime" format="yyyy-M-d H:mm:ss" showastimespan="false" runat="server" /></td>
											<td><NetRadio:Anchor id="alertDetail" target="_blank" runat="server" /></td>
										</tr>
									</ItemTemplate>
								</asp:Repeater>
<%--								<tr>
									<td align="center"><input type="checkbox" onclick="javascript:CheckboxUtil.checkAll(this, 'selection');" title="选中所有列出的项" runat="server" /></td>
									<td colspan="7"><asp:Button id="setResolved" text="设为已确认" width="90" onclick="setResolved_Click" runat="server" /> &nbsp; <span class="t3" style="position:relative; top:-2px">将选中的警告或事件设为已确认</span></td>
								</tr>--%>
								<!-- tr>
									<td></td>
									<td colspan="5">将选中项标记为已处理，并将当前时间设为判断新报警的时间断点，当前断点为 <NetRadio:SmartLabel id="timePoint" class="t1" runat="server" />。</td>
								</tr -->
								<tr>
									<td colspan="8"><NetRadio:Pagination id="p" mode="Postback" pagesize="50" class="p_left" onpageindexchanged="p_PageIndexChanged" runat="server" /></td>
								</tr>
							</table>
						</Content>
						<Footer />
            </NetRadio:CollapseView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
