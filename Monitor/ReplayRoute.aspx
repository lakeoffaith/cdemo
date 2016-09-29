<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="ReplayRoute.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Monitor.__ReplayRoute" %>

<%@ Register TagPrefix="NetRadio" TagName="SelectTagUser" Src="~/Controls/SelectTagUser.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">

    <script>
        //var fromDate=document.getElementById('<%= fromDate.UniqueID%>');
    </script>

    <NetRadio:CollapseView ID="collapseView" runat="server">
				<Header visible="false">
					<Caption id="caption">播放移动轨迹</Caption>
				</Header>
				<Content>
					<table class="grid fixed">
						<tr>
							<td width="160">选择<NetRadio:SmartLabel id="nameCalling" runat="server" /></td>
							<td><NetRadio:SelectTagUser id="tagSelector" runat="server" MaxSelectCount="3" TitleRight="◆◆ 已选对象" /></td>
						</tr>
						<tr>
							<td>活动地图</td>
							<td><NetRadio:FacilityMapDropList id="facilityMap" name="facilityMap" runat="server" /> (多个使用者时需要选择地图）</td>
						</tr>
						<tr>
							<td>时间范围</td>
							<td>
							从
							<NetRadio:CalendarBox id="fromDate" width="80" runat="server" />
								<asp:DropDownList id="fromHour" runat="server">								
								</asp:DropDownList>
								<asp:DropDownList id="fromMinute" runat="server">								
								</asp:DropDownList>
								 至
								 <NetRadio:CalendarBox id="toDate" width="80" runat="server" />
								<asp:DropDownList id="toHour" runat="server">									
								</asp:DropDownList>
								<asp:DropDownList id="toMinute" runat="server">									
								</asp:DropDownList>
								
							</td>
						</tr>
						<tr>
							<td></td>
							<td> <asp:Button id="ButtonSearch" Text="搜索" CssClass="button" onclick="buttonSearch_Click" runat="server" />
							&nbsp;&nbsp;&nbsp;&nbsp;
							<input type="button" id="play" value="播放" class="button" onclick="javascript:View();" /></td>
						</tr>
					</table>
				</Content>
    </NetRadio:CollapseView>
    <NetRadio:CollapseView ID="listPanel" runat="server">
					<Header skinId="header_empty" />
					<Content>
						<table class="grid alternate fixed">
							<thead>
								<th width="16"></th>
								<th width="180">名称</th>
								<th width="400">移动至位置</th>
								<th width="150">是否消失</th>
								<th ><NetRadio:SortButton id="updateTimeSorter" text="发生时间" sortKey="WriteTime" onclick="sorter_Click" runat="server" /></th>
								
							</thead>
							<asp:Repeater id="list" onItemCreated="list_ItemCreated" runat="server">
								<ItemTemplate>
									<tr>
										<td></td>
										<td><NetRadio:Anchor id="tagName" target="_blank" runat="server" /></td>
										<td><NetRadio:SmartLabel id="facilityName" runat="server" /> &nbsp;-&nbsp; <NetRadio:SmartLabel id="coordinatesName" runat="server" /></td>
										<td><NetRadio:SmartLabel id="isDisappeared" runat="server" /></td>
										<td><NetRadio:DateTimeLabel id="writeTime" showAsTimeSpan="false" format="yyyy-M-d H:mm:ss" runat="server" /></td>
										
									</tr>
								</ItemTemplate>
							</asp:Repeater>
							<tr>
								<td colspan="5"><NetRadio:Pagination id="p" pageSize="50" mode="Postback" onpageindexchanged="p_PageIndexChanged" class="p_left" runat="server" /></td>
							</tr>
						</table>
					</Content>
    </NetRadio:CollapseView>
</asp:Content>
